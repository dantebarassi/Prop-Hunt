using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject _playerPrefab;
    [SerializeField] GameObject _hunterPrefab;
    public int count=0;
    [Networked]
    private bool needHunter{get; set;}
    //Siempre se va a ejecutar cuando alguien se una, y esta funcion da toda la data del que se unio
    public void PlayerJoined(PlayerRef player)
    {
        //count = GameManager.instance.netPlayerCount;
        //Esto para saber si es el cliente local que se esta conectando para que pase solo esa vez
        if (player == Runner.LocalPlayer)
        {
            Debug.Log(Runner.ActivePlayers.ToList().Count);
            if (Runner.ActivePlayers.ToList().Count == 1)
            {
                Debug.Log("Hunter");
                Cursor.lockState = CursorLockMode.Locked;
                Runner.Spawn(_hunterPrefab, Vector3.zero, Quaternion.identity);
            }
            else if (Runner.ActivePlayers.ToList().Count > 1)
            {
                Debug.Log("Player");
                //Aca lo spawneo
                Cursor.lockState = CursorLockMode.Locked;
                Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity);
            }
            //Debug.Log(Runner.ActivePlayers.ToList().Count);
            //Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            //Cursor.lockState = CursorLockMode.Locked;
            //Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity);
        }
        
    }
}
