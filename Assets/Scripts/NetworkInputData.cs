using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{
    //NECESITO CLICK Y E
    public const byte MOUSEBUTTON0 = 1;
    public const byte EBUTTON = 2;
    public const byte SPACEBAR = 3;

    public NetworkButtons buttons;
    //ESTO ES LO QUE HAY QUE ACTUALIZAR CON EL CLIENTE
    public Vector3 direction;
}