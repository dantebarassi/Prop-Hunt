using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class BasicPlayer : NetworkBehaviour
{
    [SerializeField] public GameObject hunterView;
    [SerializeField] public GameObject playerView;
    public int CambioMiForma = 0;
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void Spawned()
    {
        RPC_RequestSync();
        if (!HasInputAuthority)
        {
            //ResyncNetworckValuesRpc();
            return;
        }
        Cursor.lockState = CursorLockMode.Locked;
        //base.Spawned();
        //UIManager.instance.PrenderSelector(GoHunter, GoPlayer);
        GameManager.instance.playerBasic.Add(this.gameObject);
        if (SingletonChoose.Instance.selectedMode == 1)
            GoHunter();
        else
            GoPlayer();

        

    }
    // Update is called once per frame
    void Update()
    {
        if (!HasInputAuthority)
        {
            //ResyncNetworckValuesRpc();
            return;
        }
    }
    public void GoHunter()
    {
        if (!HasInputAuthority)
        {
            Debug.Log("no tengo");
            //ResyncNetworckValuesRpc();
            return;
        }
        //GameManager.instance.RPCAddHunter(this);
        Debug.Log(GameManager.instance.gameObject);
        RPC_AddHunter(this);
        //StartCoroutine(asd());
        //GameManager.instance.RPCAddHunter(this);
        //    gameObject.GetComponent<Hunter>().enabled = true;
        //    hunterView.gameObject.SetActive(true);
    }
    //IEnumerator asd()
    //{
    //    yield return new WaitForSeconds(1);
    //    //GameManager.instance.RPCAddHunter(this);
    //    RPCAddHunter(this);
    //}
    public void GoPlayer()
    {
        Debug.Log("Entra RPDADDPLAYER1");
        if (!HasInputAuthority)
        {
            //ResyncNetworckValuesRpc();
            return;
        }
        Debug.Log("Entra RPDADDPLAYER2");
        //GameManager.instance.RPCAddPlayer(this);
        RPC_AddPlayer(this);
        //gameObject.GetComponent<Player>().enabled = true;
        //gameObject.GetComponent<Healt>().enabled = true;
        //playerView.gameObject.SetActive(true);
    }
     [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_AddHunter(BasicPlayer player)
    {
        
        RPC_ClientApplyHunter();
        RpcActiveHunterWait();
        

        //playerBasic.First().gameObject.GetComponent<Hunter>().enabled = true;
        //playerBasic.First().GetComponent<BasicPlayer>().hunterView.gameObject.SetActive(true);
        //playerBasic.First().gameObject.GetComponent<Player>().enabled = false;
        //playerBasic.First().gameObject.GetComponent<Healt>().enabled = false;
        //playerBasic.First().GetComponent<BasicPlayer>().playerView.gameObject.SetActive(false);
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
        //player.gameObject.GetComponent<Hunter>().enabled = true;
        //player.hunterView.gameObject.SetActive(true);
        //player.gameObject.GetComponent<PlayerMovementSimple>().enabled = false;
        //player.gameObject.GetComponent<Healt>().enabled = false;
        //player.playerView.gameObject.SetActive(false);
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
        // El server mira cómo está ESTE objeto y re-envía el estado a TODOS
        if (GetComponent<Hunter>().enabled)
            RPC_ClientApplyHunter();   // broadcast
        else
            RPC_ClientApplyPlayer();   // broadcast
    }

}
