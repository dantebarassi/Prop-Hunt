using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public int playersActive;

    public Hunter hunter;
    public List<PlayerMovementSimple> playerMovements = new();

    //[Networked, OnChangedRender(nameof(StartGame))]
    public bool startGame { get; set; } = false;
    public Dictionary<int,GameObject> Objetos;
    public static GameManager instance;

    [SerializeField] public GameObject deadPlace;
    
    public float timer=0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject); 
        }
        Objetos = new Dictionary<int, GameObject>();
    }
    private void Update()
    {
        //if (startGame)
        //    timer += Runner.DeltaTime;
        //if(timer >= 60f)
        //    UIManager.instance.SetVictoryScreen(playerMovements.First().gameObject);
    }
    public void FillWithObjects(int idTryEntry, GameObject objectsTryEntry)
    {
        if (!Objetos.ContainsKey(idTryEntry))
        {
            Objetos.Add(idTryEntry, objectsTryEntry);
            Debug.Log("Se agrego" + idTryEntry + "   " + objectsTryEntry);
        }
            
    }
    public GameObject MeshSelector(int Id)
    {
        return Objetos.GetValueOrDefault(Id);
    }
    public void SetPlayerSpectating(PlayerMovementSimple playerMovementSimple)
    {
        FindObjectOfType<CameraBehavior>().SetSpectating();
        playerMovementSimple.SetInputsAllowed(false);
    }

    //public void WhoWins() => Debug.Log(kills);
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RpcHunterGetKill()
    //{
    //    // The code inside here will run on the client which owns this object (has state and input authority).
    //    //Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
    //    kills++;
    //    if (kills >= 4)
    //        UIManager.instance.SetVictoryScreen(hunter.gameObject);
    //}
    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RpcStartGame()
    //{
    //    // The code inside here will run on the client which owns this object (has state and input authority).
    //    //Debug.Log("Received DealDamageRpc on StateAuthority, modifying Networked variable");
    //    startGame=true;
    //}
    //public void StartGame() => Debug.Log("STAAAART");
}
