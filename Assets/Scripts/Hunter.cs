using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class Hunter : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    public static Hunter LocalPlayer { get; private set; }
    public float speed = 3;
    [SerializeField] float _speed;

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

    public int kills { get; set; } = 0;
    public float timer = 0;
    ChangeDetector _changeDetector;
    bool hunterCan = false;
    public bool alreadyStartTime = false;
    public bool isMoving=false;

    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }

    public override void Spawned()
    {

        GameManager.instance.hunter = this;
        if (!HasStateAuthority)
        {

            return;
        }
        LocalPlayer = this;
        hunter = this;
        speed = 15;
        Camera = Camera.main;
        Camera.main.GetComponent<CameraBehavior>().target = transform;
        Camera.main.GetComponent<CameraBehavior>().enabled = true;

    }

    public override void FixedUpdateNetwork()
    {

        if (GetInput(out NetworkInputData data))
        {
            if (!HasInputAuthority) return;
            if (Runner.ActivePlayers.Count() >= 2 && hunterCan) _cc.Move(_speed * data.direction * Runner.DeltaTime);
            data.direction.Normalize();
            if (data.direction.sqrMagnitude == 0) isMoving = false;
            else isMoving = true;

            if (hunterCan)
            {
                UIManager.instance.HunterWait(false);


                if (Input.GetKeyDown(KeyCode.Space))
                {
                    _cc.Jump();
                }


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
                                health.DealDamageRpc(Damage, this);
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
    }

    void Update()
    {
        if (!HasStateAuthority) return;
        if (Runner.ActivePlayers.Count() < 2)
        {
            hunterCan = false;
            UIManager.instance.HunterWait(true);
            GameManager.instance.startTimer = false;
        }
        else
        {
            if(!hunterCan)
            {
                StartCoroutine(HunterCanMove());
                GameManager.instance.startTimer = true;
            }
        }

        
    }

    public override void Render()
    {
        if (_changeDetector == null) return;
    }

    public void Jump()
    {
        Debug.Log("Jump");
        var velocity = _rb.velocity;
        velocity.y += jumpForce;
        _rb.velocity = velocity;
        _jumpPressed = false;
    }

    public void StartHunterCanMove()
    {
        StartCoroutine(HunterCanMove());
    }
    IEnumerator HunterCanMove()
    {
        yield return new WaitForSeconds(10f);

        GameManager.instance.hunterCan = true;
        hunterCan = true;
        RpcCanMove();

    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcCanMove()
    {
        GameManager.instance.RpcHunterCanMove();
    }

}
