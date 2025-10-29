using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;
using System.Linq;

public class BasicPlayer : NetworkBehaviour
{
    [SerializeField] public GameObject hunterView;
    [SerializeField] public GameObject playerView;
    public int CambioMiForma = 0;
    public bool pause=false;
    public override void Spawned()
    {
        RPC_RequestSync();
        if (!HasInputAuthority)
        {
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.instance.playerBasic.Add(this.gameObject);
        if (SingletonChoose.Instance.selectedMode == 1)
            GoHunter();
        else
            GoPlayer();

        

    }
    void Update()
    {
        if (!HasInputAuthority)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(pause)
            {
                UIManager.instance.Pause(false);
                Cursor.lockState = CursorLockMode.Locked;
            }
            else
            {
                UIManager.instance.Pause(true);
                Cursor.lockState = CursorLockMode.None;
            }
        }
        if(Runner.ActivePlayers.Count()==1)
            UIManager.instance.WaitingOthers(true);
        else
            UIManager.instance.WaitingOthers(false);
    }
    public void GoHunter()
    {
        if (!HasInputAuthority)
        {
            Debug.Log("no tengo");
            return;
        }
        Debug.Log(GameManager.instance.gameObject);
        RPC_AddHunter(this);
    }
    public void GoPlayer()
    {
        Debug.Log("Entra RPDADDPLAYER1");
        if (!HasInputAuthority)
        {
            return;
        }
        Debug.Log("Entra RPDADDPLAYER2");
        RPC_AddPlayer(this);
    }
     [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_AddHunter(BasicPlayer player)
    {
        RPC_ClientApplyHunter();
        RpcActiveHunterWait();
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcActiveHunterWait()
    {
        UIManager.instance.HunterWait(true);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_AddPlayer(BasicPlayer player)
    {
        RPC_ClientApplyPlayer();
        Debug.Log("Entra RPDADDPLAYER3");
        player.gameObject.GetComponent<PlayerMovementSimple>().enabled = true;
        player.gameObject.GetComponent<Healt>().enabled = true;
        player.playerView.gameObject.SetActive(true);
        player.gameObject.GetComponent<Hunter>().enabled = false;
        player.hunterView.gameObject.SetActive(false);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientApplyHunter()
    {
        var go = gameObject;
        go.GetComponent<Hunter>().enabled = true;
        if (hunterView) hunterView.SetActive(true);

        var mover = go.GetComponent<PlayerMovementSimple>();
        if (mover) mover.enabled = false;

        var hp = go.GetComponent<Healt>();
        if (hp) hp.enabled = false;

        if (playerView) playerView.SetActive(false);
    }
    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_ClientApplyPlayer()
    {
        var go = gameObject;
        var mover = go.GetComponent<PlayerMovementSimple>();
        if (mover) mover.enabled = true;

        var hp = go.GetComponent<Healt>();
        if (hp) hp.enabled = true;

        if (playerView) playerView.SetActive(true);

        var hunter = go.GetComponent<Hunter>();
        if (hunter) hunter.enabled = false;

        if (hunterView) hunterView.SetActive(false);
    }
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    private void RPC_RequestSync()
    {
        if (GetComponent<Hunter>().enabled)
            RPC_ClientApplyHunter();
        else
            RPC_ClientApplyPlayer();
    }

}
