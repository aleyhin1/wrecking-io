using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Spawner : NetworkBehaviour, INetworkRunnerCallbacks
{
    [SerializeField] private Transform[] _spawnPositions;
    [SerializeField] private NetworkPrefabRef _playerPrefab;
    [SerializeField] private NetworkPrefabRef _ballPrefab;
    private int _spawnCount = 0;
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
            Quaternion rotation = Quaternion.Euler(0, _spawnRotations[_spawnCount], 0);

            NetworkObject networkPlayerObject = runner.Spawn(_playerPrefab, spawnPosition, Quaternion.identity, player);
            NetworkObject networkBallObject = runner.Spawn(_ballPrefab, Vector3.zero, Quaternion.identity, player);

            networkPlayerObject.GetComponent<Rigidbody>().MoveRotation(rotation);
            networkBallObject.GetComponent<BallMovement>().targetCarObject = networkPlayerObject;

            Runner.SetPlayerObject(player, networkPlayerObject);
            networkPlayerObject.GetComponent<PlayerDataScript>().PlayerBall = networkBallObject.gameObject;

            _spawnCount++;
            
        }

        ActivatePlayerCamera(player);
    }


    private void ActivatePlayerCamera(PlayerRef playerRef)
    {
        if (Runner.LocalPlayer != playerRef)
        {
            return;
        }

        Runner.TryGetPlayerObject(playerRef, out NetworkObject networkPlayerObject);
        var playerCamera = networkPlayerObject.gameObject.GetComponentInChildren<Camera>(includeInactive: true);
        playerCamera.gameObject.SetActive(true);
    }

    #region NetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
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
