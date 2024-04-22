using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    //Siempre se va a ejecutar cuando alguien se una
    public void PlayerJoined(PlayerRef player)
    {
        
    }
}
