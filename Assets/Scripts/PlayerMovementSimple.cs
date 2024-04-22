using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovementSimple : NetworkBehaviour
{
    public float speed=3;

    private float _moveY;
    private float _moveX;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _moveY = Input.GetAxisRaw("Horizontal");
        _moveX = Input.GetAxisRaw("Vertical");
    }

    public override void FixedUpdateNetwork()
    {
        transform.position += (Vector3.forward * _moveY) * speed * Runner.DeltaTime;
        transform.position += (Vector3.right * _moveX*-1) * speed * Runner.DeltaTime;
    }
}
