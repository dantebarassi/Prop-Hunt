using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Hunter : NetworkBehaviour
{
    public static Hunter LocalPlayer { get; private set; }
    public float speed = 3;

    private float _moveY;
    private float _moveX;
    private Rigidbody _rb;
    public float jumpForce = 4;
    private bool _jumpPressed;
    private Vector3 velocity;
    Hunter hunter;
    private Vector3 cameraDir;
    Camera Camera;
    private Renderer _renderer;
    public float Damage = 40;
    public bool attack=false;

    // Start is called before the first frame update
    void Start()
    {
        //myMesh = myVisual.gameObject.GetComponent<MeshFilter>();
    }

    public override void Spawned()
    {
        //base.Spawned();
        _rb = GetComponent<Rigidbody>();
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
        _moveY = Input.GetAxisRaw("Horizontal");
        _moveX = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _jumpPressed = true;
        }
        //Aca hacer el raycast y poner adentro 
        //MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>(); // Obt�n el MeshRenderer del objeto impactado
        //Collider collider = hit.collider; // El Collider ya lo tenemos desde hit.collider
        if(!attack)
        {
            //Raycast
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                attack = true;
                Ray ray = hunter.Camera.ScreenPointToRay(Input.mousePosition);
                ray.origin += hunter.Camera.transform.forward;
                Debug.DrawRay(ray.origin, ray.direction, Color.red, 5f);
                if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction, out var hit))
                {
                    if (hit.transform.TryGetComponent<Healt>(out var health))
                    {
                        health.DealDamageRpc(Damage);
                    }
                }
            }
        }
        else
        {
            float countToAttack = 0;
            countToAttack += Runner.DeltaTime;
            if (countToAttack >= 6)
                attack = false;
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
}