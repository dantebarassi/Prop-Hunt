using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class HunterView : NetworkBehaviour
{
    private Renderer _renderer;
    Animator _animator;
    public static HunterView Local { get; private set; }


    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Local = this;
        }
        _renderer = GetComponentInChildren<Renderer>();
        _animator = GetComponentInChildren<Animator>();
    }
    public override void FixedUpdateNetwork()
    {
        _animator.SetBool("isRunning", Hunter.LocalPlayer._rb.velocity.SetY0().sqrMagnitude > 0);
        _animator.SetBool("isAttacking", Hunter.LocalPlayer.attack);
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
