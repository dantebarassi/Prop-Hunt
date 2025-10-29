using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerMovementSimple : NetworkBehaviour
{
    private NetworkCharacterController _cc;
    [SerializeField] float _speed;

    ChangeDetector _changeDetector;

    public static PlayerMovementSimple LocalPlayer { get; private set; }
    public float speed = 3;

    private float _moveY;
    private float _moveX;
    public Rigidbody _rb { get; private set; }
    public float jumpForce = 4;
    private bool _jumpPressed;
    public bool _changeFormPressed { get; private set; }
    private Vector3 velocity;

    private Vector3 cameraDir;
    PlayerMovementSimple PlayerMovement;
    Camera Camera;

    public PlayerView playerView;


    //Mover todo esto a player view

    private Renderer _renderer;
    public GameObject thisGameObjectOriginal;
    [SerializeField] GameObject _myView;

    
    public int OtherId;

    public MeshFilter OtherMeshRenderer;
    public BoxCollider OtherCollider;
    public Material OtherMaterial;
    public bool inpusAllowed=true;
    public bool isMoving;
 
    [Networked, OnChangedRender(nameof(NetChangeForm))]
    public int netId { get; set; }
    public void NetChangeForm()
    {
        OtherMeshRenderer = GameManager.instance.MeshSelector(netId).GetComponent<MeshFilter>();
        OtherCollider = GameManager.instance.MeshSelector(netId).GetComponent<BoxCollider>();
        OtherMaterial = GameManager.instance.MeshSelector(netId).GetComponent<Renderer>().material;
        ChangeForm(OtherMeshRenderer, OtherCollider, OtherMaterial);
    }
    private void Awake()
    {
        _cc = GetComponent<NetworkCharacterController>();
    }


    void Start()
    {
        thisGameObjectOriginal = this.gameObject;
        _changeFormPressed = false;

    }

    public override void Spawned()
    {

        LocalPlayer = this;
        playerView = GetComponentInChildren<PlayerView>();
        speed = 15;
        PlayerMovement = this;
        Camera = Camera.main;

        Camera.main.GetComponentInParent<MouseLook>().myTarget = transform;
        Camera.main.GetComponentInParent<MouseLook>().enabled = true;

    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    void ResyncNetworckValuesRpc()
    {
        StartCoroutine(ResyncValues());
    }

    IEnumerator ResyncValues()
    {
        var id = netId;
        netId = 0;
        yield return new WaitForSeconds(0.1f);
        netId = id;
    }


    public override void FixedUpdateNetwork()
    {
        if (GetInput(out NetworkInputData data))
        {

            if (!HasStateAuthority) return;
            if (inpusAllowed)
            {
                data.direction.Normalize();
                if (Runner.ActivePlayers.Count() >= 2) _cc.Move(_speed * data.direction * Runner.DeltaTime);
                if (data.direction.sqrMagnitude == 0) isMoving = false;
                else isMoving = true;

                if (data.buttons.IsSet(NetworkInputData.SPACEBAR))
                {
                    _cc.Jump();
                }

                //Raycast
                Ray ray = PlayerMovement.Camera.ScreenPointToRay(Input.mousePosition);
                ray.origin += PlayerMovement.Camera.transform.forward;

                Debug.DrawRay(ray.origin, ray.direction*3f, Color.red, 1f);
                if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction, out var hit, 3f))
                {
                    if (hit.transform.TryGetComponent<Objetos>(out var objectHitted))
                    {
                        OtherId = objectHitted.id;
                        if (Input.GetKeyDown(KeyCode.E))
                        {
                            netId = OtherId;
                            OtherCollider = objectHitted.GetComponent<BoxCollider>();
                            OtherMeshRenderer = objectHitted.GetComponent<MeshFilter>();
                            OtherMaterial = objectHitted.GetComponent<Renderer>().material;
                            _changeFormPressed = true;
                        }
                    }
                    else
                        OtherId = 0;
                }
            }
        }
    }

    public void Movement()
    {
        if (_jumpPressed) Jump();
        if (_changeFormPressed) ChangeForm(OtherMeshRenderer, OtherCollider, OtherMaterial);

        if (_moveY != 0 || _moveX != 0)
        {
            cameraDir = Camera.main.transform.forward;
            cameraDir.y = 0;
            this.transform.forward = cameraDir;
            _rb.velocity += (((this.transform.forward * _moveX) * speed * Runner.DeltaTime) + ((this.transform.right * _moveY) * speed * Runner.DeltaTime));
            if (Mathf.Abs(_rb.velocity.magnitude) > speed)
            {
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
    public void ChangeForm(MeshFilter mesh, BoxCollider collider, Material material)
    {
        Debug.Log("ChangeForm");
        
        _myView.GetComponent<MeshFilter>().mesh = mesh.mesh;

        this.gameObject.GetComponent<BoxCollider>().size = collider.size;
        this.gameObject.GetComponent<BoxCollider>().center = collider.center;
        this.gameObject.transform.localScale = collider.gameObject.transform.localScale;

        _myView.GetComponent<Renderer>().material = material;

        speed = 0;

        StartCoroutine(FinishChangeForm());
    }


    IEnumerator FinishChangeForm()
    {
        yield return new WaitForSeconds(2f);
        Debug.Log("FinishChangeForm");
        speed = 15;
        _changeFormPressed = false;
    }
    public void BackNormal()
    {
        Debug.Log("BackNormal");
        netId = 0;
        NetChangeForm();
    }
    public void SetInputsAllowed(bool inputs)
    {
        inpusAllowed = inputs;
    }
}