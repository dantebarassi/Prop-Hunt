using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class Hunter : NetworkBehaviour
{
    public static Hunter LocalPlayer { get; private set; }
    public float speed = 3;

    private float _moveY;
    private float _moveX;
    public Rigidbody _rb;
    public float jumpForce = 4;
    private bool _jumpPressed;
    private Vector3 velocity;
    Hunter hunter;
    private Vector3 cameraDir;
    Camera Camera;
    private Renderer _renderer;
    public float Damage = 40;
    public bool attack=false;
    float countToAttack = 0;
    bool hunterCan = false;
    [Networked, OnChangedRender(nameof(WhoWins))]
    public int kills { get; set; }
    public float timer = 0;
    // Start is called before the first frame update
    void Start()
    {
        //myMesh = myVisual.gameObject.GetComponent<MeshFilter>();
    }

    public override void Spawned()
    {
        //base.Spawned();
        _rb = GetComponent<Rigidbody>();
        GameManager.instance.hunter = this;
        if (!HasStateAuthority)
        {
            //ResyncNetworckValuesRpc();
            return;
        }
        LocalPlayer = this;
        hunter = this;
        speed = 15;
        Camera = Camera.main;
        Camera.main.GetComponent<CameraBehavior>().target = transform;
        //var velocity = _rb.velocity;
    }
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //void ResyncNetworckValuesRpc()
    //{
    //    StartCoroutine(ResyncValues());
    //}
    //
    //IEnumerator ResyncValues()
    //{
    //    var id = netId;
    //    netId = 0;
    //    yield return new WaitForSeconds(0.1f);
    //    netId = id;
    //}


    void Update()
    {
        if (!HasStateAuthority) return;
        if (Runner.ActivePlayers.Count() < 2)
        {
            hunterCan = false;
            UIManager.instance.HunterWait();
        }
        else
        {
            if(!hunterCan)
                StartCoroutine(HunterCanMove());
        }
        if (hunterCan)
        {
            _moveY = Input.GetAxisRaw("Horizontal");
            _moveX = Input.GetAxisRaw("Vertical");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _jumpPressed = true;
            }
            
            //Aca hacer el raycast y poner adentro 
            //MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>(); // Obtén el MeshRenderer del objeto impactado
            //Collider collider = hit.collider; // El Collider ya lo tenemos desde hit.collider
            if (!attack)
            {
                //Raycast
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    attack = true;
                    countToAttack = 0;
                    Ray ray = hunter.Camera.ScreenPointToRay(Input.mousePosition);
                    ray.origin += hunter.Camera.transform.forward;
                    Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
                    if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction, out var hit))
                    {
                        if (hit.transform.TryGetComponent<Healt>(out var health))
                        {
                            health.DealDamageRpc(Damage,this);
                        }
                    }
                }
            }
            else
            {
                countToAttack += Runner.DeltaTime;
                if (countToAttack >= 2f)
                    attack = false;
            }
        }
    }

    public override void FixedUpdateNetwork()
    {
        //transform.position += (Vector3.forward * _moveY) * speed * Runner.DeltaTime;
        //transform.position += (Vector3.right * _moveX*-1) * speed * Runner.DeltaTime;
        //_rb.MovePosition((Vector3.forward * _moveY) * speed * Runner.DeltaTime);
        //_rb.MovePosition((Vector3.right * _moveX * -1) * speed * Runner.DeltaTime);

        //_rb.velocity += ;
        if (!HasStateAuthority) return;
        if(hunterCan)
        {
            timer += Runner.DeltaTime;
            //Debug.Log(timer);
            if (timer >= 600f)
                UIManager.instance.SetVictoryScreen();
        }
        Movement();
    }

    public void Movement()
    {
        if (_jumpPressed) Jump();

        if (_moveY != 0 || _moveX != 0)
        {
            cameraDir = Camera.main.transform.forward;
            cameraDir.y = 0;
            this.transform.forward = cameraDir;
            _rb.velocity += (((this.transform.forward * _moveX) * speed * Runner.DeltaTime) + ((this.transform.right * _moveY) * speed * Runner.DeltaTime));
            if (Mathf.Abs(_rb.velocity.magnitude) > speed)
            {
                //var velocity = Vector3.ClampMagnitude(_rb.velocity, speed);
                velocity.y = _rb.velocity.y;
                _rb.velocity = velocity;
            }
        }
        else
        {
            var velocity = _rb.velocity;
            velocity.x = 0;
            velocity.z = 0;
            _rb.velocity = velocity;
        }
    }

    public void Jump()
    {
        Debug.Log("Jump");
        var velocity = _rb.velocity;
        velocity.y += jumpForce;
        _rb.velocity = velocity;
        _jumpPressed = false;
    }
    //[Networked, OnChangedRender(nameof(activePlayers))]
    //public float NetworkActivePlayers { get; set; }
    //
    //void activePlayers()
    //{
    //    Debug.Log(NetworkActivePlayers);
    //}
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RpcPlayerJoin()
    //{
    //    // The code inside here will run on the client which owns this object (has state and input authority).
    //    //Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
    //    NetworkActivePlayers ++;
    //}

    IEnumerator HunterCanMove()
    {
        yield return new WaitForSeconds(10f);
        //Debug.Log("StartMoveHunter");
        //velocity = 0f;
        hunterCan = true;
        UIManager.instance.HunterStart();
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcHunterGetKill()
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        //Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        kills++;
        if (kills >= Runner.ActivePlayers.Count()-1)
            UIManager.instance.SetVictoryScreen(this.gameObject);
    }
    public void WhoWins() => Debug.Log(kills);
}
