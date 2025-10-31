using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Healt : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHealth { get; set; } = 100;
    PlayerView _pv;
    public ParticleSystem particlesHit;
    public MeshRenderer view;

    private void Start()
    {
        _pv = GetComponentInChildren<PlayerView>();
    }

    void HealthChanged()
    {
        Debug.Log($"Health changed to: {NetworkedHealth}");
    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void DealDamageRpc(float damage, Hunter hunter)
    {
        //Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        if(NetworkedHealth - damage > 0)
        {
            NetworkedHealth -= damage;
            if (_pv != null)
            {
                _pv.Hit();
                particlesHit.Play();
                //RPC_PlayHitFX();
            }
        }
        else
        {
            NetworkedHealth -= damage;
            if (HasStateAuthority)
            {
                GameManager.instance.RpcHunterGetKill();
            }
            //hunter.RpcHunterGetKill();
            this.gameObject.GetComponent<PlayerMovementSimple>().transform.position = GameManager.instance.deadPlace.transform.position;
            //GameManager.instance.SetPlayerSpectating(this.gameObject.GetComponent<PlayerMovementSimple>());
            view.enabled = false;
            //RPC_Die();
        }
    }
    //[Rpc(RpcSources.All, RpcTargets.All)]
    //private void RPC_PlayHitFX()
    //{
    //    particlesHit.Play();
    //}
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //private void RPC_Die()
    //{
    //    view.enabled = false;
    //}
}
