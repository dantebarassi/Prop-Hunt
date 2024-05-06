using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovementSimple : NetworkBehaviour
{
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
    //[SerializeField] GameObject myVisual;
    //[SerializeField] MeshFilter myMesh;
    //[SerializeField] Collider myCollider;
    //[SerializeField] Material myMaterial;
    [Networked, OnChangedRender(nameof(NetChangeForm))]
    public int netId { get; set; }
    public int OtherId;
    //public MeshRenderer netMeshRenderer { get; set; }
    //public Collider netCollider { get; set; }
    public MeshFilter OtherMeshRenderer;
    public Collider OtherCollider;
    public Material OtherMaterial;
    public bool inpusAllowed=true;
    //Donde va?
    //Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
    //Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;
    //Lo de aca hace que le avise a todos qeu tiene que cambiar ese variable 
    public void NetChangeForm()
    {
        OtherMeshRenderer = GameManager.instance.MeshSelector(netId).GetComponent<MeshFilter>();
        OtherCollider = GameManager.instance.MeshSelector(netId).GetComponent<Collider>();
        OtherMaterial = GameManager.instance.MeshSelector(netId).GetComponent<Material>();
        ChangeForm(OtherMeshRenderer, OtherCollider, OtherMaterial);
    }

    // Start is called before the first frame update
    void Start()
    {
        thisGameObjectOriginal = this.gameObject;
        //myMesh = myVisual.gameObject.GetComponent<MeshFilter>();
    }

    public override void Spawned()
    {
        //base.Spawned();
        _rb = GetComponent<Rigidbody>();
        GameManager.instance.playerMovements.Add(this);
        if (!HasStateAuthority)
        {
            ResyncNetworckValuesRpc();
            return;
        }
        LocalPlayer = this;
        playerView = GetComponentInChildren<PlayerView>();
        speed = 15;
        PlayerMovement = this;
        Camera = Camera.main;
        Camera.main.GetComponent<CameraBehavior>().target = transform;
        //var velocity = _rb.velocity;
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


    void Update()
    {
        if (!HasStateAuthority) return;
        if(inpusAllowed)
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

            //Raycast
            Ray ray = PlayerMovement.Camera.ScreenPointToRay(Input.mousePosition);
            ray.origin += PlayerMovement.Camera.transform.forward;
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
            if (Runner.GetPhysicsScene().Raycast(ray.origin, ray.direction, out var hit))
            {
                if (hit.transform.TryGetComponent<Objetos>(out var objectHitted))
                {
                    OtherId = objectHitted.id;
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        netId = OtherId;
                        OtherCollider = objectHitted.GetComponent<Collider>();
                        OtherMeshRenderer = objectHitted.GetComponent<MeshFilter>();
                        OtherMaterial = objectHitted.GetComponent<Material>();
                        _changeFormPressed = true;
                    }
                }
                else
                    OtherId = 0;
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
        Movement();
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
    public void ChangeForm(MeshFilter mesh, Collider collider, Material material)
    {
        Debug.Log("ChangeForm");
        
        _myView.GetComponent<MeshFilter>().mesh = mesh.mesh;

        //Destroy(this.GetComponent<BoxCollider>());

        Collider col = this.gameObject.GetComponent<Collider>();
        col = collider;

        _myView.GetComponent<Renderer>().material = material;

        speed = 0;
        //NetChangeForm();
        StartCoroutine(FinishChangeForm());
    }
    //public void FinishChangeForm()
    //{
    //    Debug.Log("FinishChangeForm");
    //    speed = 3;
    //    _changeFormPressed = false;
    //}

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