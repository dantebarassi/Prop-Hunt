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

    public int propsActive,huntersActive;

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
    public int kills=0;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject); 
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

    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void RpcHunterGetKill()
    {

        kills++;
        RecountTeams();
        RpcWhoWins();

    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcHunterCanMove()
    {

        RpcWhoWins();

    }
    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RpcStartGame()
    {

        startGame=true;
    }
    public void StartGame() => Debug.Log("STAAAART");
    [Rpc]
    public void RpcWhoWins()
    {
        if (kills >= propsActive && huntersActive >= 1 && propsActive >=1 && huntersActive+ propsActive >1)
            RpcSetVictoryScreen(true);
        if (hunterCan && !alreadyStartTime)
        {
            StartCoroutine(TimeToWinProps());
        }
    }
    [Rpc]
    private void RpcSetVictoryScreen(bool isHunter)
    {
        UIManager.instance.SetVictoryScreen(isHunter);
    }

    public IEnumerator TimeToWinProps()
    {
        Debug.Log("EsperandoGanar");
        alreadyStartTime = true;
        yield return new WaitForSeconds(40f);
        Debug.Log("Gane");
        RpcSetVictoryScreen(false);
    }

    private void RecountTeams()
    {
        propsActive = 0;
        huntersActive = 0;

        // agarra TODOS los BasicPlayer que estén en la escena
        var players = FindObjectsOfType<BasicPlayer>();

        foreach (var bp in players)
        {
            // si ese BasicPlayer tiene Hunter habilitado → hunter
            var h = bp.GetComponent<Hunter>();
            if (h != null && h.enabled)
            {
                huntersActive++;
            }
            else
            {
                // si no es hunter → lo contamos como prop
                propsActive++;
            }
        }

        Debug.Log($"RECOUNT -> props={propsActive} | hunters={huntersActive} | kills={kills}");
    }

    //[Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    //public void RpcRegisterRole(bool isProp)
    //{
    //    if (isProp)
    //        propsActive++;
    //    else
    //        huntersActive++;
    //
    //    // ahora les mando a todos los números buenos
    //    RpcSyncCounts(propsActive, huntersActive);
    //}
    //
    //// server -> todos
    //[Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    //public void RpcSyncCounts(int props, int hunters)
    //{
    //    propsActive = props;
    //    huntersActive = hunters;
    //    //Debug.Log($"SYNC COUNTS -> props: {propsActive} | hunters: {huntersActive} | kills: {kills}");
    //}

    //[Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    //public void RpcAddPlayer(bool isProp)
    //{
    //    if (isProp)
    //        propsActive++;
    //    else
    //        huntersActive++;
    //}
}
