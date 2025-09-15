using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using UnityEngine.UI;

public class BasicPlayer : NetworkBehaviour
{
    [SerializeField] public GameObject hunterView;
    [SerializeField] public GameObject playerView;
    public int CambioMiForma=0;
    // Start is called before the first frame update
    void Start()
    {

    }
    public override void Spawned()
    {
        if (!HasInputAuthority)
        {
            //ResyncNetworckValuesRpc();
            return;
        }
        //base.Spawned();
        UIManager.instance.PrenderSelector(GoHunter, GoPlayer);
        GameManager.instance.playerBasic.Add(this.gameObject);
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
        if (!HasInputAuthority)
        {
            //ResyncNetworckValuesRpc();
            return;
        }
        //GameManager.instance.RPCAddPlayer(this);
        RPC_AddPlayer(this);
        //gameObject.GetComponent<Player>().enabled = true;
        //gameObject.GetComponent<Healt>().enabled = true;
        //playerView.gameObject.SetActive(true);
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_AddHunter(BasicPlayer player)
    {
        player.gameObject.GetComponent<Hunter>().enabled = true;
        player.hunterView.gameObject.SetActive(true);
        player.gameObject.GetComponent<Player>().enabled = false;
        player.gameObject.GetComponent<Healt>().enabled = false;
        player.playerView.gameObject.SetActive(false);

        //playerBasic.First().gameObject.GetComponent<Hunter>().enabled = true;
        //playerBasic.First().GetComponent<BasicPlayer>().hunterView.gameObject.SetActive(true);
        //playerBasic.First().gameObject.GetComponent<Player>().enabled = false;
        //playerBasic.First().gameObject.GetComponent<Healt>().enabled = false;
        //playerBasic.First().GetComponent<BasicPlayer>().playerView.gameObject.SetActive(false);
    }
    [Rpc(RpcSources.All, RpcTargets.All, HostMode = RpcHostMode.SourceIsHostPlayer)]
    public void RPC_AddPlayer(BasicPlayer player)
    {
        player.gameObject.GetComponent<Player>().enabled = true;
        player.gameObject.GetComponent<Healt>().enabled = true;
        player.playerView.gameObject.SetActive(true);
        player.gameObject.GetComponent<Hunter>().enabled = false;
        player.hunterView.gameObject.SetActive(false);
    }
}
