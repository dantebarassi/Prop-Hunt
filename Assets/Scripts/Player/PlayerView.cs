using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerView : NetworkBehaviour
{
    private Renderer _renderer;
    public Animator _animator;
    public static PlayerView Local { get; private set; }
    public PlayerMovementSimple _player;

    public override void Spawned()
    {
        if(HasStateAuthority)
        {
            Local = this;
        }
        _renderer = GetComponentInChildren<Renderer>();
        _animator = GetComponent<Animator>();
        _player = GetComponentInParent<PlayerMovementSimple>();
    }
    public override void FixedUpdateNetwork()
    {
        _animator.SetBool("isRunning", _player.isMoving);
        _animator.SetBool("isTranforming", PlayerMovementSimple.LocalPlayer._changeFormPressed);
    }
    public void Hit()
    {
        _animator.SetTrigger("Hit");
    }
}
