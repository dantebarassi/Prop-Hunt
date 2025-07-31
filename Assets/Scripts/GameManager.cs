using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class GameManager : NetworkBehaviour
{
    public List<PlayerMovementSimple> playerMovements = new();
    public List<Hunter> hunters = new();
    public List<GameObject> playerBasic = new();
    public Hunter hunter;

    public int playersActive;

    //[Networked, OnChangedRender(nameof(StartGame))]
    public bool startGame { get; set; } = false;
    public Dictionary<int,GameObject> Objetos;
    public static GameManager instance;
    public bool hunterCan = false;
    public bool startTimer = false;
    [SerializeField] public GameObject deadPlace;
    public bool alreadyStartTime = false;

    public float timer { get; set; } = 0;
    [Networked, OnChangedRender(nameof(StartGame))]
    public int ContadorListos { get; set; } = 0;
    public int kills { get; set; }
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
        FindObjectOfType<MouseLook>().SetSpectating();
        playerMovementSimple.SetInputsAllowed(false);
    }
    public void EstaListo()
    {
        ContadorListos++;
        Comenzar();
    }
    public void Comenzar()
    {
        if (ContadorListos >= Runner.ActivePlayers.Count())
            startGame = true;
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
    public void StartGame() => Debug.Log("STAAAART");
    public void WhoWins()
    {
        if (kills >= Runner.ActivePlayers.Count() - 1)
            RpcSetVictoryScreen(hunter);
    }
    [Rpc]
    private void RpcSetVictoryScreen(Hunter hunter)
    {
        UIManager.instance.SetVictoryScreen(hunter);
    }
    [Rpc]
    private void RpcSetVictoryScreen()
    {
        UIManager.instance.SetVictoryScreen();
    }
}
