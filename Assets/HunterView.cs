using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class HunterView : NetworkBehaviour
{
    private Renderer _renderer;
    [SerializeField] SkinnedMeshRenderer _skinnedRenderer, _skinnedRenderer2;
    public Animator _animator;
    public Hunter _hunter;
    public static HunterView Local { get; private set; }


    public override void Spawned()
    {
        if (HasStateAuthority)
        {
            Local = this;
        }
        _renderer = GetComponentInChildren<Renderer>();
        _animator = GetComponentInChildren<Animator>();
        if (HasStateAuthority)
        {
            _skinnedRenderer.enabled = false;
            _skinnedRenderer2.enabled = false;
        }
        //_skinnedRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        _hunter = GetComponentInParent<Hunter>();
    }
    public override void FixedUpdateNetwork()
    {
        _animator.SetBool("isRunning", _hunter.isMoving);
        _animator.SetBool("isAttacking", _hunter.attack);
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
