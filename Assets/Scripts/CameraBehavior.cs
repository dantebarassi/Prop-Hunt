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

        transform.position = target.position + new Vector3(0, 2.261f, 0);

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        verticalRotation -= mouseY * MouseSensitivity;
        verticalRotation = Mathf.Clamp(verticalRotation, -70f, 70f);

        horizontalRotation += mouseX * MouseSensitivity;

        transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
        target.transform.rotation = Quaternion.Euler(0, horizontalRotation, 0);


        if (target == null)
        {
            return;
        }
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

    [SerializeField] float _yMaxRotation = 80, _yMinRotation = -80, _yRotation;
    public void Rotation(float xAxis, float yAxis)
    {
        _yRotation += yAxis;

        _yRotation = Mathf.Clamp(_yRotation, _yMinRotation, _yMaxRotation);

        transform.rotation = Quaternion.Euler(-_yRotation, xAxis, 0f);
    }
}
