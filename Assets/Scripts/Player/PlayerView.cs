using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerView : NetworkBehaviour
{
    private Renderer _renderer;
    Animator _animator;
    public static PlayerView Local { get; private set; }
    Player _player;

    public override void Spawned()
    {
        if(HasStateAuthority)
        {
            Local = this;
        }
        _renderer = GetComponentInChildren<Renderer>();
        _animator = GetComponentInChildren<Animator>();
        _player = GetComponentInParent<Player>();
    }
    public override void FixedUpdateNetwork()
    {
        _animator.SetBool("isRunning", PlayerMovementSimple.LocalPlayer._rb.velocity.SetY0().sqrMagnitude > 0);
        //_animator.SetBool("isRunning", PlayerMovementSimple.LocalPlayer._rb.velocity.magnitude == 0);
        _animator.SetBool("isTranforming", PlayerMovementSimple.LocalPlayer._changeFormPressed);
    }
    public void Hit()
    {
        _animator.SetTrigger("Hit");
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
