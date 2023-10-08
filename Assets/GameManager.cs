using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using static GameState;

public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static GameManager Instance { get; set; }
    public static GameState State { get; set; }
    public static StartGameScript StartGameScript { get; set; }
    public static CameraManager CameraManager { get; set; }

    public NetworkDebugStart starter;
    public GameSettings Settings { get; set; } = GameSettings.Default;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            State = GetComponent<GameState>();
            StartGameScript = GetComponent<StartGameScript>();
            CameraManager = GetComponent<CameraManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public override void Spawned()
    {
        base.Spawned();
        if (Runner.IsServer)
        {
            State.Server_SetState(EGameState.Pregame);
        }
        Runner.AddCallbacks(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        runner.RemoveCallbacks(this);
        starter.Shutdown();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_StartArena()
    {
        Debug.Log("RPC_StartArena");
        State.Server_SetState(EGameState.Play);
    }

    #region NetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player) { }
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
