using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkStartBridge : MonoBehaviour
{
    public NetworkDebugStart Starter;
    public TMPro.TMP_InputField HostCodeField;
    public TMPro.TMP_InputField JoinCodeField;

    private void OnEnable()
    {
        HostCodeField.text = Starter.DefaultRoomName;
        JoinCodeField.text = Starter.DefaultRoomName;
    }

    public void SetCode(string code)
    {
        Starter.DefaultRoomName = code;
    }

    public void StartHost()
    {
        if (string.IsNullOrWhiteSpace(Starter.DefaultRoomName))
            Starter.DefaultRoomName = RoomCode.Create();
        Starter.StartHost();
    }

    public void StartClient()
    {
        Starter.StartClient();
    }
}
