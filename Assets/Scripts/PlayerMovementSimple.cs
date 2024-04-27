using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovementSimple : NetworkBehaviour
{
    public float speed = 3;

    private float _moveY;
    private float _moveX;
    private Rigidbody _rb;
    public float jumpForce = 4;
    private bool _jumpPressed;
    private bool _changeFormPressed;
    private Vector3 velocity;

    private Vector3 cameraDir;
    PlayerMovementSimple PlayerMovement;
    Camera Camera;
    private Renderer _renderer;

    [Networked, OnChangedRender(nameof(NetChangeForm))]
    public MeshRenderer netMeshRenderer { get; set; }
    public Collider netCollider { get; set; }
    public MeshRenderer OtherMeshRenderer;
    public Collider OtherCollider;
    //Donde va?
    //Quaternion cameraRotationY = Quaternion.Euler(0, Camera.transform.rotation.eulerAngles.y, 0);
    //Vector3 move = cameraRotationY * new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")) * Runner.DeltaTime * PlayerSpeed;
    //Lo de aca hace que le avise a todos qeu tiene que cambiar ese variable 
    public void NetChangeForm() => netMeshRenderer = OtherMeshRenderer;
    // Start is called before the first frame update
    void Start()
    {

    }

    public override void Spawned()
    {
        //base.Spawned();
        _rb = GetComponent<Rigidbody>();
        if (!HasStateAuthority) return;
        speed = 15;
        PlayerMovement = this;
        Camera = Camera.main;
        Camera.main.GetComponent<CameraBehavior>().target = transform;
        //var velocity = _rb.velocity;
    }

    // Update is called once per frame
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
        //MeshRenderer meshRenderer = hit.collider.GetComponent<MeshRenderer>(); // Obtén el MeshRenderer del objeto impactado
        //Collider collider = hit.collider; // El Collider ya lo tenemos desde hit.collider
        if (Input.GetKeyDown(KeyCode.E))
        {
            _changeFormPressed = true;
        }
        //Raycast
        Ray ray = PlayerMovement.Camera.ScreenPointToRay(Input.mousePosition);
        ray.origin += PlayerMovement.Camera.transform.forward;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            Debug.DrawRay(ray.origin, ray.direction, Color.red, 1f);
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
        if (_changeFormPressed) ChangeForm(OtherMeshRenderer, OtherCollider);

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
    public void ChangeForm(MeshRenderer mesh, Collider collider)
    {
        Debug.Log("ChangeForm");
        speed = 0;
    }
    public void FinishChangeForm()
    {
        speed = 3;
        _changeFormPressed = false;
    }
}