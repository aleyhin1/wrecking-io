using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInputPoller : MonoBehaviour, INetworkRunnerCallbacks
{
    public GameObject JoystickBackground;
    public GameObject JoystickHandle;

    private void OnEnable()
    {
        var Runner = FindObjectOfType<NetworkRunner>();
        Runner.AddCallbacks(this);
    }


    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        if (Input.touchCount > 0)
        {
            NetworkInputData inputData = new NetworkInputData();

            inputData.JoystickBackgroundPosition = JoystickBackground.transform.position;
            inputData.JoystickHandlePosition = JoystickHandle.transform.position;

            input.Set(inputData);
        }
    }

    public void OnDisable()
    {
        var Runner = FindObjectOfType<NetworkRunner>();
        if (Runner != null)
            Runner.RemoveCallbacks(this);
    }

    #region NetworkRunnerCallbacks
    void INetworkRunnerCallbacks.OnPlayerJoined(Fusion.NetworkRunner runner, Fusion.PlayerRef player) { }
    void INetworkRunnerCallbacks.OnConnectedToServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnConnectFailed(NetworkRunner runner, Fusion.Sockets.NetAddress remoteAddress, Fusion.Sockets.NetConnectFailedReason reason) { }
    void INetworkRunnerCallbacks.OnShutdown(NetworkRunner runner, ShutdownReason shutdownReason) { }
    void INetworkRunnerCallbacks.OnDisconnectedFromServer(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnPlayerLeft(NetworkRunner runner, PlayerRef player) { }
    void INetworkRunnerCallbacks.OnInputMissing(NetworkRunner runner, PlayerRef player, NetworkInput input) { }
    void INetworkRunnerCallbacks.OnConnectRequest(NetworkRunner runner, NetworkRunnerCallbackArgs.ConnectRequest request, byte[] token) { }
    void INetworkRunnerCallbacks.OnUserSimulationMessage(NetworkRunner runner, SimulationMessagePtr message) { }
    void INetworkRunnerCallbacks.OnSessionListUpdated(NetworkRunner runner, List<SessionInfo> sessionList) { }
    void INetworkRunnerCallbacks.OnCustomAuthenticationResponse(NetworkRunner runner, Dictionary<string, object> data) { }
    void INetworkRunnerCallbacks.OnReliableDataReceived(NetworkRunner runner, PlayerRef player, System.ArraySegment<byte> data) { }
    void INetworkRunnerCallbacks.OnSceneLoadDone(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnSceneLoadStart(NetworkRunner runner) { }
    void INetworkRunnerCallbacks.OnHostMigration(NetworkRunner runner, HostMigrationToken hostMigrationToken) { }
    #endregion NetworkRunnerCallbacks
}
