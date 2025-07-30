using Fusion;
using UnityEngine;

public struct NetworkInputData : INetworkInput
{

    public const byte MOUSEBUTTON0 = 1;
    public const byte EBUTTON = 2;
    public const byte SPACEBAR = 3;

    public NetworkButtons buttons;
    public Vector3 direction;
}