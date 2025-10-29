using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;
using Fusion.Sockets;
using System;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined, INetworkRunnerCallbacks
{

    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPoints;
    public int count=0;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    public void PlayerJoined(PlayerRef player)
    {
        if (player == Runner.LocalPlayer)
        {
            Vector3 spawnPosition = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length - 1)].transform.position;
            Debug.Log(spawnPosition);
            NetworkObject networkPlayerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
        }
    }
    public void OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (_spawnedCharacters.TryGetValue(player, out NetworkObject networkObject))
        {
            runner.Despawn(networkObject);
            _spawnedCharacters.Remove(player);
        }
    }

    private bool _mouseButton0;
    private bool _eButton;
    private bool _spacebar;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            _mouseButton0 = _mouseButton0 |= true;
        if (Input.GetKeyDown(KeyCode.E))
            _eButton = _eButton |= true;
        if (Input.GetKeyDown(KeyCode.Space))
            _spacebar = _spacebar |= true;
    }
    public void OnInput(NetworkRunner runner, NetworkInput input)
    {
        var data = new NetworkInputData();
    
        if (Input.GetKey(KeyCode.W))
            data.direction += Camera.main.transform.forward;
    
        if (Input.GetKey(KeyCode.S))
            data.direction -= Camera.main.transform.forward;
    
        if (Input.GetKey(KeyCode.A))
            data.direction -= Camera.main.transform.right;
    
        if (Input.GetKey(KeyCode.D))
            data.direction += Camera.main.transform.right;
    
        data.direction.y = 0;
    
        data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
        _mouseButton0 = false;
        data.buttons.Set(NetworkInputData.EBUTTON, _eButton);
        _eButton = false;
        data.buttons.Set(NetworkInputData.SPACEBAR, _spacebar);
        _spacebar = false;
    
        input.Set(data);
    }

    public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    public void OnConnectedToServer(NetworkRunner runner) { }
    public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    public void OnSceneLoadDone(NetworkRunner runner) { }
    public void OnSceneLoadStart(NetworkRunner runner) { }
    public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {

    }
    public async void ExitToMenu()
    {
        var runner = FindObjectOfType<NetworkRunner>();
        if (runner)
        {
            bool soyMaster = runner.GameMode == GameMode.Shared &&
                             (runner.IsSharedModeMasterClient || runner.IsServer);
            await runner.Shutdown();
        }

        Cursor.visible = true;
    }

}
