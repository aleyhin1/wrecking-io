using Fusion;
using Fusion.Editor;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

public class Spawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static List<CharacterData> ActiveCharacters = new List<CharacterData>();

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
        if (Runner.IsServer)
        {
            Vector3 spawnPosition = _spawnPositions[_spawnCount].position;
            Quaternion spawnRotation = Quaternion.Euler(0, _spawnRotations[_spawnCount], 0);

            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);

            Runner.SetPlayerObject(player, networkPlayerObject);
            SetCarPositionAndRotation(networkPlayerObject, spawnPosition, spawnRotation);

            NetworkObject networkBallObject = runner.Spawn(_ballPrefab, Vector3.zero, Quaternion.identity, player);
            InitBall(networkBallObject, spawnPosition, spawnRotation);

            CharacterData playerData = new CharacterData(player, networkPlayerObject, networkBallObject);
            ActiveCharacters.Add(playerData);

            RPC_BindKCC(networkPlayerObject, networkBallObject);
            RPC_BindRope(networkPlayerObject, networkBallObject);

            _spawnCount++;
        }
    }

    private void SetCarPositionAndRotation(NetworkObject playerCar, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        KCC playerCarKcc = playerCar.GetComponent<KCC>();

        playerCarKcc.SetPosition(spawnPosition);
        playerCarKcc.SetLookRotation(spawnRotation);
    }

    private void InitBall(NetworkObject ballObject, Vector3 spawnPosition, Quaternion spawnRotation)
    {
        Vector3 ballOffset = TransformUtils.GetOffsetWorldVector(10, spawnRotation);
        ballObject.GetComponent<KCC>().SetPosition(spawnPosition + ballOffset);
        ballObject.GetComponent<KCC>().SetLookRotation(spawnRotation);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_BindKCC(NetworkObject carObject, NetworkObject ballObject)
    {
        BindKCC(carObject, ballObject);
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    private void RPC_BindRope(NetworkObject carObject, NetworkObject ballObject)
    {
        BindRope(carObject, ballObject);
    }

    public void BindKCC(NetworkObject carObject, NetworkObject ballObject)
    {
        BallMovement ballScript = ballObject.GetComponent<BallMovement>();
        KCC carKCC = carObject.GetComponent<KCC>();
        ballScript._carKcc = carKCC;
    }

    public void BindRope(NetworkObject carObject, NetworkObject ballObject)
    {
        RopeScript ropeScript = ballObject.GetComponentInChildren<RopeScript>();
        KCC carKCC = carObject.GetComponent<KCC>();
        KCC ballKCC = ballObject.GetComponent<KCC>();
        ropeScript.BindRope(carKCC, ballKCC);
    }

    public void ActivatePlayerCamera()
    {
        Runner.TryGetPlayerObject(Runner.LocalPlayer, out NetworkObject networkPlayerObject);
        var playerCamera = networkPlayerObject.gameObject.GetComponentInChildren<Camera>(includeInactive: true);
        playerCamera.gameObject.SetActive(true);
    }

    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player)
    {
        if (Runner.IsServer)
        {
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

                NetworkObject botCarObject = Runner.Spawn(_botCarPrefab, spawnPosition, Quaternion.identity);
                SetCarPositionAndRotation(botCarObject, spawnPosition, spawnRotation);

                NetworkObject botBallObject = Runner.Spawn(_botBallPrefab, Vector2.zero, Quaternion.identity);
                InitBall(botBallObject, spawnPosition, spawnRotation);
                RPC_BindKCC(botCarObject, botBallObject);
                RPC_BindRope(botCarObject, botBallObject);

                int randomCarColor = GetRandomIndex(GetCarColorsLength(botCarObject));
                int randomPlayerColor = GetRandomIndex(GetPlayerColorsLength(botCarObject));
                int randomBallColor = GetRandomIndex(GetBallColorsLength(botBallObject));
                SetBotCarColor(botCarObject, randomCarColor);
                SetBotPlayerColor(botCarObject, randomPlayerColor);
                SetBotBallColor(botBallObject, randomBallColor);


                _spawnCount++;
                _botCount++;

                CharacterData botData = new CharacterData(botCarObject, botBallObject, _botCount, randomCarColor, randomPlayerColor, 
                    randomBallColor);
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

                ActiveCharacters.Remove(lastBotData);

                _spawnCount--;
                _botCount--;
            }
        }
    }

    private void SetBotCarColor(NetworkObject carObject, int carColorIndex)
    {
        CarColorChanger carColorChanger = carObject.GetComponent<CarColorChanger>();
        carColorChanger.RPC_ChangeCarColor(carColorIndex);
    }

    private void SetBotPlayerColor(NetworkObject carObject, int playerColorIndex)
    {
        CarColorChanger carColorChanger = carObject.GetComponent<CarColorChanger>();
        carColorChanger.RPC_ChangePlayerColor(playerColorIndex);
    }
    
    private void SetBotBallColor(NetworkObject ballObject, int ballColorIndex)
    {
        BallColorChanger ballColorChanger = ballObject.GetComponent<BallColorChanger>();
        ballColorChanger.RPC_ChangeBallModel(ballColorIndex);
    }

    private int GetCarColorsLength(NetworkObject carObject)
    {
        CarColorChanger carColorChanger = carObject.GetComponent<CarColorChanger>();
        return carColorChanger.CarMaterials.Length;
    }

    private int GetPlayerColorsLength(NetworkObject carObject)
    {
        CarColorChanger carColorChanger = carObject.GetComponent<CarColorChanger>();
        return carColorChanger.PlayerMaterials.Length;
    }
    
    private int GetBallColorsLength(NetworkObject ballObject)
    {
        BallColorChanger ballColorChanger = ballObject.GetComponent<BallColorChanger>();
        return ballColorChanger.BallModels.Length;
    }

    private int GetRandomIndex(int maxLength)
    {
        return Random.Range(0, maxLength);
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
