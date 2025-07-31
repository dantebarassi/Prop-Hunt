using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class Healt : NetworkBehaviour
{
    [Networked, OnChangedRender(nameof(HealthChanged))]
    public float NetworkedHealth { get; set; } = 100;
    PlayerView _pv;

    private void Start()
    {
        _pv = GetComponentInChildren<PlayerView>();
    }

    void HealthChanged()
    {
        Debug.Log($"Health changed to: {NetworkedHealth}");
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void DealDamageRpc(float damage, Hunter hunter)
    {
        // The code inside here will run on the client which owns this object (has state and input authority).
        Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
        if(NetworkedHealth - damage > 0)
        {
            NetworkedHealth -= damage;
            if (_pv != null)
                _pv.Hit();
        }
        else
        {
            //Poner en modo espectador y decir que mori
            //Quiza mandar al gamemanager que si muero sumar uno y si se llega a tanto tiempo ganan los que se esconden
            //FindObjectOfType<Hunter>().RpcPlayerJoin();
            NetworkedHealth -= damage;
            hunter.RpcHunterGetKill();
            this.gameObject.GetComponent<PlayerMovementSimple>().transform.position = GameManager.instance.deadPlace.transform.position;
            GameManager.instance.SetPlayerSpectating(this.gameObject.GetComponent<PlayerMovementSimple>());
        }
    }
}
