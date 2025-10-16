using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fusion;
using System.Linq;

public class PlayerSpawn : SimulationBehaviour, IPlayerJoined
{
    private NetworkRunner _runner;
    //[SerializeField] GameObject _playerPrefab;
    //[SerializeField] GameObject _hunterPrefab;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] Transform[] _spawnPoints;
    public int count=0;
    private Dictionary<PlayerRef, NetworkObject> _spawnedCharacters = new Dictionary<PlayerRef, NetworkObject>();

    //Siempre se va a ejecutar cuando alguien se una, y esta funcion da toda la data del que se unio
    public void PlayerJoined(PlayerRef player)
    {
        //count = GameManager.instance.netPlayerCount;
        //Esto para saber si es el cliente local que se esta conectando para que pase solo esa vez
        if (player == Runner.LocalPlayer)
        {
            //Debug.Log(Runner.ActivePlayers.ToList().Count);
            //if (Runner.ActivePlayers.ToList().Count == 1)
            //{
            //    Debug.Log("Hunter");
            //    Cursor.lockState = CursorLockMode.Locked;
            //    Runner.Spawn(_hunterPrefab, _spawnPoints[0].transform.position, Quaternion.identity);
            //}
            //else if (Runner.ActivePlayers.ToList().Count > 1)
            //{
            //    Debug.Log("Player");
            //    //Aca lo spawneo
            //    Cursor.lockState = CursorLockMode.Locked;
            //    Runner.Spawn(_playerPrefab, _spawnPoints[Random.Range(1,_spawnPoints.Length-1)].transform.position, Quaternion.identity);
            //}
            //Cursor.lockState = CursorLockMode.Locked;
            Vector3 spawnPosition = _spawnPoints[UnityEngine.Random.Range(0, _spawnPoints.Length - 1)].transform.position;
            Debug.Log(spawnPosition);
            //Vector3 spawnPosition = new Vector3((player.RawEncoded % runner.Config.Simulation.PlayerCount) * _spawnPoints[0].transform.position.x, _spawnPoints[0].transform.position.y, _spawnPoints[0].transform.position.z);
            NetworkObject networkPlayerObject = Runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            //NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, _spawnPoints[0].transform.position, Quaternion.identity, player);
            // Keep track of the player avatars for easy access
            _spawnedCharacters.Add(player, networkPlayerObject);
            //Debug.Log(Runner.ActivePlayers.ToList().Count);
            //Debug.LogWarning("AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
            //Cursor.lockState = CursorLockMode.Locked;
            //Runner.Spawn(_playerPrefab, Vector3.zero, Quaternion.identity);
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
    async void StartGame(GameMode mode)
    {
        // Create the Fusion runner and let it know that we will be providing user input
        _runner = gameObject.AddComponent<NetworkRunner>();
        _runner.ProvideInput = true;

        // Create the NetworkSceneInfo from the current scene
        //var scene = SceneRef.FromIndex(SceneManager.GetActiveScene().buildIndex);
        //var sceneInfo = new NetworkSceneInfo();
        //if (scene.IsValid)
        //{
        //    sceneInfo.AddSceneRef(scene, LoadSceneMode.Additive);
        //}
        //
        //// Start or join (depends on gamemode) a session with a specific name
        //await _runner.StartGame(new StartGameArgs()
        //{
        //    GameMode = mode,
        //    SessionName = "TestRoom",
        //    Scene = scene,
        //    SceneManager = gameObject.AddComponent<NetworkSceneManagerDefault>()
        //});
    }
    //private void OnGUI()
    //{
    //    if (_runner == null)
    //    {
    //        if (GUI.Button(new Rect(0, 0, 200, 40), "Host"))
    //        {
    //            StartGame(GameMode.Host);
    //        }
    //        if (GUI.Button(new Rect(0, 40, 200, 40), "Join"))
    //        {
    //            StartGame(GameMode.Client);
    //        }
    //    }
    //}
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
    //public void OnInput(NetworkRunner runner, NetworkInput input)
    //{
    //    var data = new NetworkInputData();
    //
    //    if (Input.GetKey(KeyCode.W))
    //        data.direction += Camera.main.transform.forward;
    //
    //    if (Input.GetKey(KeyCode.S))
    //        data.direction -= Camera.main.transform.forward;
    //
    //    if (Input.GetKey(KeyCode.A))
    //        data.direction -= Camera.main.transform.right;
    //
    //    if (Input.GetKey(KeyCode.D))
    //        data.direction += Camera.main.transform.right;
    //
    //    data.direction.y = 0;
    //
    //    data.buttons.Set(NetworkInputData.MOUSEBUTTON0, _mouseButton0);
    //    _mouseButton0 = false;
    //    data.buttons.Set(NetworkInputData.EBUTTON, _eButton);
    //    _eButton = false;
    //    data.buttons.Set(NetworkInputData.SPACEBAR, _spacebar);
    //    _spacebar = false;
    //
    //    input.Set(data);
    //}
    //public void OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    //public void OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    //public void OnConnectedToServer(NetworkRunner runner) { }
    ////public void OnDisconnectedFromServer(NetworkRunner runner, NetDisconnectReason reason) { }
    ////public void OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    ////public void OnConnectFailed(NetworkRunner runner, NetAddress remoteAddress, NetConnectFailedReason reason) { }
    //public void OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    //public void OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    //public void OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    //public void OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    //public void OnSceneLoadDone(NetworkRunner runner) { }
    //public void OnSceneLoadStart(NetworkRunner runner) { }
    //public void OnObjectExitAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    //public void OnObjectEnterAOI(NetworkRunner runner, NetworkObject obj, PlayerRef player) { }
    //public void OnReliableDataReceived(NetworkRunner runner, PlayerRef player, ReliableKey key, ArraySegment<byte> data) { }
    //public void OnReliableDataProgress(NetworkRunner runner, PlayerRef player, ReliableKey key, float progress) { }
}
