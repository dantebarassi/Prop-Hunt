using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    
    [Header("Camera")]
    Camera _myCamera;

    [Header("Target")]
    public Transform myTarget;
    //[SerializeField] Transform _overTheShoulder;
    //public float maxY;
    //public float minY;
    //public float maxY2;
    //public float minY2;


    [Header("Mouse Settings")]
    [SerializeField] float _mouseSensitivity = 100;

    float _minDistance;
    float _maxDistance=5;
    float _minRotation=-80;
    float _maxRotation=80;


    [Header("Raycast Settings")]
    [SerializeField] float _hitOffset=0.1f;

    float _mouseX, _mouseY;
    //public float zoomDistance=2f;

    Vector3 _direction;
    Vector3 _camPos;

    Ray _ray;
    RaycastHit _rHit;
    bool _isCameraBlocked;

    private List<PlayerMovementSimple> _spectatingList = new List<PlayerMovementSimple>();
    private bool _spectating = false;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        transform.forward = myTarget.forward;
        _mouseX = transform.eulerAngles.y;
        _mouseY = transform.eulerAngles.x;
        _myCamera = Camera.main;
    }
    [SerializeField] LayerMask layerMask;
    private void FixedUpdate()
    {
        //Genero un "Rayo invisible" desde mi posicion hacia la direccion de la camara
        _ray = new Ray(transform.position, _direction);

        //Paso el rayo a un SphereCast para que me genere un radio en esa direccion
        //y mediante la variable _rHit obtengo los datos contra lo que colision (si es que colisiono)
        //Ademas la ejecucion de este metodo me devuelve un booleano que lo guardo en _isCameraBlocked
        _isCameraBlocked = Physics.SphereCast(_ray, 0.1f, out _rHit, _maxDistance, layerMask);
    }

    private void LateUpdate()
    {
        if (myTarget == null)
        {
            return;
        }
        //Actualizar la posicion y rotacion del Socket
        UpdateCameraRotation(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));

        //Actualizar la ubicacion de la camara
        UpdateSpringArm();

    }

    void UpdateCameraRotation(float xAxi, float yAxi)
    {
        transform.position = myTarget.position;

        if (xAxi == 0 && yAxi == 0) return;

        //Tomo los inputs directos del mouse y lo sumo a lo que ya vengo acumulando de antes

        if (xAxi != 0)
        {
            _mouseX += xAxi * _mouseSensitivity * Time.deltaTime;

            //"Capeo" el valor de mi acumulacion en eje horizontal entre 360 y -360
            if (_mouseX < 360 || _mouseX > -360)
            {
                //_mouseX = transform.eulerAngles.y;
                _mouseX -= 360 * Mathf.Sign(_mouseX);
            }
        }

        if (yAxi != 0)
        {
            _mouseY += yAxi * _mouseSensitivity * Time.deltaTime;

            //Clampeo la rotacion del mouse en eje vertical para tener un maximo y minimo de vision hacia arriba y abajo
            _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);
        }

        //Aplicamos la rotacion
        transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);
    }

    void UpdateSpringArm()
    {
        //Tomo la direccion contraria hacia donde mira el objeto que contiene este script
        _direction = -transform.forward;

        //Si esta bloqueada la vision
        if (_isCameraBlocked)
        {
            //Me voy a fijar si donde voy a poner la camara es demasiado cerca
            var dirTest = (_rHit.point - transform.position) + (_rHit.normal * _hitOffset);

            //Si es muy cerca, la fixeo un poco mas lejos
            if (dirTest.sqrMagnitude <= _minDistance * _minDistance)
            {
                _camPos = transform.position + _direction * _minDistance;
            }
            else
            {
                //Pongo la camara en la posicion donde toque algo que la bloquea
                _camPos = transform.position + dirTest;
            }
        }
        else //Sino
        {
            //Pongo la camara en la distancia correcta a mi personaje (determinado por _maxDistance)
            _camPos = transform.position + _direction * _maxDistance;
        }

        //Le paso esa posicion nueva generada
        _myCamera.transform.position = _camPos;
        //Y le digo a mi camara que mire hacia el personaje
        _myCamera.transform.LookAt(transform.position);
    }
    public void SetSpectating()
    {
        _spectating = true;
        _spectatingList = new List<PlayerMovementSimple>(FindObjectsOfType<PlayerMovementSimple>());
        myTarget = GetRandomSpectatingTarget();
    }
    private Transform GetRandomSpectatingTarget()
    {
        if (_spectatingList.Count == 1)
        {
            return myTarget;
        }
        return _spectatingList.Find(x => x.transform.GetChild(0) != myTarget).transform.GetChild(0);
    }
}
