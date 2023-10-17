using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;
using static GameState;

public class GameManager : NetworkBehaviour, INetworkRunnerCallbacks
{
    public static GameManager Instance { get; private set; }
    public static GameState State { get; private set; }
    public static CameraManager CameraManager { get; private set; }
    public static ArenaManager ArenaManager { get; private set; }

    public GameObject DeathScreen;
    public GameObject PreGameScreen;
    public NetworkDebugStart Starter;
    public GameObject Joystick;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            State = GetComponent<GameState>();
            CameraManager = GetComponent<CameraManager>();
            ArenaManager = GetComponent<ArenaManager>();
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
            State.Server_SetState(EGameState.Off);
        }

        Runner.AddCallbacks(this);
    }

    public override void Despawned(NetworkRunner runner, bool hasState)
    {
        base.Despawned(runner, hasState);
        runner.RemoveCallbacks(this);
        Starter.Shutdown();
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_StartArena()
    {
        State.Server_SetState(EGameState.Play);
    }

    void INetworkRunnerCallbacks.OnPlayerJoined(NetworkRunner runner, PlayerRef player)
    {
        if (runner.IsServer)
        {
            State.Server_SetState(EGameState.Pregame);
        }
    }

    #region NetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
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
