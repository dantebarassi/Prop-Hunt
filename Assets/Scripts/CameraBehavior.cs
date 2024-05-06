using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehavior : MonoBehaviour
{
    public Transform target;

    public float MouseSensitivity = 10f;

    private float verticalRotation;
    private float horizontalRotation;

    //Spectador
    private float _step;
    public float Speed = 1f;

    private List<PlayerMovementSimple> _spectatingList = new List<PlayerMovementSimple>();
    private bool _spectating = false;
    private Vector3 _offset = new Vector3(0, 5, 0);

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.position;

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);


        //Spectador
        if (target == null)
        {
            return;
        }

        //_step = Speed * Vector2.Distance(target.position, transform.position) * Time.deltaTime;
        //
        //Vector2 pos = Vector2.MoveTowards(transform.position, target.position + _offset, _step);
        //transform.position = pos;
    }


    //Spectador

    public void SetSpectating()
    {
        _spectating = true;
        _spectatingList = new List<PlayerMovementSimple>(FindObjectsOfType<PlayerMovementSimple>());
        target = GetRandomSpectatingTarget();
    }

    /// <summary>
    /// Return a random and different than current player transform.
    /// </summary>
    /// <returns></returns>
    private Transform GetRandomSpectatingTarget()
    {
        if (_spectatingList.Count == 1)
        {
            return target;
        }
        return _spectatingList.Find(x => x.transform.GetChild(0) != target).transform.GetChild(0);
    }

    /// <summary>
    /// Return next or prev player transform.
    /// </summary>
    /// <param name="to"></param>
    /// <returns></returns>
    private Transform GetNextOrPrevSpectatingTarget(int to)
    {
        int currentIndex = _spectatingList.IndexOf(target.GetComponentInParent<PlayerMovementSimple>());

        if (currentIndex + to >= _spectatingList.Count)
        {
            return _spectatingList[0].transform.GetChild(0);
        }
        else if (currentIndex + to < 0)
        {
            return _spectatingList[_spectatingList.Count - 1].transform.GetChild(0);
        }
        else
        {
            return _spectatingList[currentIndex + to].transform.GetChild(0);
        }
    }

    private void Update()
    {
        if (_spectating)
        {
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                target = GetNextOrPrevSpectatingTarget(1);
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                target = GetNextOrPrevSpectatingTarget(-1);
            }
        }
    }


    //[Header("Mouse Settings")]
    //[SerializeField] float _mouseSensitivity = 100;
    //
    //[Header("Camera Distance")]
    //[Range(0.25f, 2f), SerializeField]
    //float _minDistance;
    //
    //[Range(2f, 10f), SerializeField]
    //float _maxDistance;
    //
    //[Header("Clamp Settings")]
    //[SerializeField] float _minRotation;
    //[SerializeField] float _maxRotation;
    //float _mouseX, _mouseY;
    //void Start()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //
    //    transform.forward = target.forward;
    //
    //    _mouseX = transform.eulerAngles.y;
    //    _mouseY = transform.eulerAngles.x;
    //}
    //
    //// Update is called once per frame
    //void Update()
    //{
    //    
    //}
    //private void LateUpdate()
    //{
    //    UpdateCameraRotation(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
    //
    //    UpdateSpringArm();
    //}
    //void UpdateCameraRotation(float xAxi, float yAxi)
    //{
    //    transform.position = _myTarget.position;
    //
    //    if (xAxi == 0 && yAxi == 0) return;
    //
    //    if (xAxi != 0)
    //    {
    //        _mouseX += xAxi * _mouseSensitivity * Time.deltaTime;
    //
    //        if (_mouseX > 360 || _mouseX < -360)
    //        {
    //            _mouseX -= 360 * Mathf.Sign(_mouseX);
    //        }
    //    }
    //
    //    if (yAxi != 0)
    //    {
    //        _mouseY += yAxi * _mouseSensitivity * Time.deltaTime;
    //
    //        _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);
    //    }
    //
    //    transform.rotation = Quaternion.Euler(-_mouseY, _mouseX, 0);
    //}
    //
    //void UpdateSpringArm()
    //{
    //    _direction = -transform.forward;
    //
    //    if (_isCameraBlocked)
    //    {
    //        var dirTest = (_rHit.point - transform.position) + (_rHit.normal * _hitOffset);
    //
    //        if (dirTest.sqrMagnitude <= _minDistance * _minDistance)
    //        {
    //            _camPos = transform.position + _direction * _minDistance;
    //        }
    //        else
    //        {
    //            _camPos = transform.position + dirTest;
    //        }
    //    }
    //    else
    //    {
    //        _camPos = transform.position + _direction * _maxDistance;
    //    }
    //
    //    _myCamera.transform.position = _camPos;
    //    //Y le digo a mi camara que mire hacia el personaje
    //    _myCamera.transform.LookAt(transform.position);
    //}

    //private void LateUpdate()
    //{
    //    //Player.LocalPlayer
    //    FollowTarget();
    //}

    //void FollowTarget()
    //{
    //    if (target == null) return;
    //
    //    transform.position = (transform.position.SetXAxis(target.position.x));
    //}
}
