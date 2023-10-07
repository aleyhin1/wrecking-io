using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalInputPoller : MonoBehaviour, INetworkRunnerCallbacks
{
    private Vector2? touchPosition;
    private Camera _camera;

    private void Start()
    {
        _camera = FindObjectOfType<Camera>();
    }

    void INetworkRunnerCallbacks.OnInput(NetworkRunner runner, NetworkInput input)
    {
        NetworkInputData inputData = new NetworkInputData();

        if (Input.touchCount > 0)
        {
            if (Input.GetTouch(0).phase == TouchPhase.Began)
                touchPosition = Input.GetTouch(0).position;

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
                touchPosition = null;

            if (touchPosition != null)
            {
                Vector2 lookVectorPixelCord = Input.GetTouch(0).position - (Vector2)touchPosition;
                inputData.LookDirection = _camera.ScreenToWorldPoint(lookVectorPixelCord).normalized;
            }
        }  
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
    #endregion
}
