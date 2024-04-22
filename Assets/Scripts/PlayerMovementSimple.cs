using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerMovementSimple : NetworkBehaviour
{
    public float speed=3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!HasStateAuthority) return;
            transform.position += (Vector3.up * Input.GetAxisRaw("Horizontal")) * speed * Time.deltaTime;
    }
}
