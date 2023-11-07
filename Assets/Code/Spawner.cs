using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class Spawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    //public Dictionary<PlayerRef, NetworkObject> BallObjects = new Dictionary<PlayerRef, NetworkObject>();
    //public Stack<NetworkObject> botObjects = new Stack<NetworkObject>();
    public List<CharacterData> ActiveCharacters = new List<CharacterData>();

    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _ballPrefab;
    [SerializeField] private NetworkPrefabRef _botCarPrefab;
    [SerializeField] private NetworkPrefabRef _botBallPrefab;
    private int _spawnCount = 0;
    private int _botCount = 0;
    private int[] _spawnRotations = { -90, 90, 0, -180, -45, 45, -135, 135 };


    public override void Spawned()
    {
        base.Spawned();
        Runner.AddCallbacks(this);
    }

    public void OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            Vector3 spawnPosition = _spawnPositions[_spawnCount].position;
            Quaternion spawnRotation = Quaternion.Euler(0, _spawnRotations[_spawnCount], 0);

            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            Runner.SetPlayerObject(player, networkPlayerObject);
            SetCarPositionAndRotation(networkPlayerObject, spawnPosition, spawnRotation);

            NetworkObject networkBallObject = runner.Spawn(_ballPrefab, Vector3.zero, Quaternion.identity, player);
            InitBall(networkBallObject, player, spawnPosition, spawnRotation);

            CharacterData playerData = new CharacterData(player, networkPlayerObject, networkBallObject);
            ActiveCharacters.Add(playerData);

            _spawnCount++;
        }

        if (Runner.LocalPlayer == player)
        {
            StartCoroutine(ActivatePlayerCamera());
        }
    }

    private void SetCarPositionAndRotation(NetworkObject playerCar, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        KCC playerCarKcc = playerCar.GetComponent<KCC>();

        playerCarKcc.SetPosition(spawnPosition);
        playerCarKcc.SetLookRotation(spawnRotation);
    }

    private void InitBall(NetworkObject ballObject, PlayerRef player, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        BallMovement ballMovementScript = ballObject.GetComponent<BallMovement>();
        ballMovementScript.TargetPlayer = player;

        Vector3 ballOffset = TransformUtils.GetOffsetWorldVector(10, spawnRotation);
        ballObject.GetComponent<KCC>().SetPosition(spawnPosition + ballOffset);
        ballObject.GetComponent<KCC>().SetLookRotation(spawnRotation);
    }


    private IEnumerator ActivatePlayerCamera()
    {
        yield return new WaitForSeconds(.5f);

        Runner.TryGetPlayerObject(Runner.LocalPlayer, out NetworkObject networkPlayerObject);
        var playerCamera = networkPlayerObject.gameObject.GetComponentInChildren<Camera>(includeInactive: true);
        playerCamera.gameObject.SetActive(true);
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (Runner.IsServer)
        {
            //NetworkObject leftPlayerCar = Runner.GetPlayerObject(player);
            //BallObjects.TryGetValue(player, out NetworkObject leftBall);
            CharacterData leftPlayerData = ActiveCharacters.Find(x => x.PlayerRef == player);
            
            Runner.Despawn(leftPlayerData.CarObject);
            Runner.Despawn(leftPlayerData.BallObject);
        }
    }

    public void AddBot()
    {
        if (Runner.IsServer)
        {
            int AllowedBotCount = 8 - ActiveCharacters.Count;
            if (AllowedBotCount > 0)
            {
                Vector3 spawnPosition = _spawnPositions[_spawnCount].position;
                Quaternion spawnRotation = Quaternion.Euler(0, _spawnRotations[_spawnCount], 0);

                NetworkObject botObject = Runner.Spawn(_botCarPrefab, spawnPosition, Quaternion.identity);
                SetCarPositionAndRotation(botObject, spawnPosition, spawnRotation);

                NetworkObject botBallObject = Runner.Spawn(_botBallPrefab, Vector2.zero, Quaternion.identity);

                _spawnCount++;
                _botCount++;

                CharacterData botData = new CharacterData(PlayerRef.None, botObject, botBallObject, _botCount);
                ActiveCharacters.Add(botData);
            }
        }
    }

    public void RemoveBot()
    {
        if (Runner.IsServer)
        {
            if (_botCount != 0)
            {
                CharacterData lastBotData = ActiveCharacters.Find( x => x.BotIndex == _botCount );

                Runner.Despawn(lastBotData.CarObject);
                Runner.Despawn(lastBotData.BallObject);

                _spawnCount--;
                _botCount--;
            }
        }
    }

    #region NetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    #endregion
}
