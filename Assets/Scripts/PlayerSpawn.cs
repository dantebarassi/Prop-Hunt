using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    [SerializeField] GameObject _playerPrefab;
    //Siempre se va a ejecutar cuando alguien se una, y esta funcion da toda la data del que se unio
    public void PlayerJoined(PlayerRef player)
    {
        //Esto para saber si es el cliente local que se esta conectando para que pase solo esa vez
        if(player == Runner.LocalPlayer)
        {
            //Aca lo spawneo
            Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
