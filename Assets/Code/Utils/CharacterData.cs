using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public PlayerRef PlayerRef { get; private set; }
    public NetworkObject CarObject { get; private set; }
    public NetworkObject BallObject { get; private set; }
    public int? BotIndex { get; private set; }

    public CharacterData(PlayerRef playerRef, NetworkObject carObject, NetworkObject ballObject)
    {
        PlayerRef = playerRef;
        CarObject = carObject;  
        BallObject = ballObject;
        BotIndex = null;
    }

    public CharacterData(NetworkObject carObject, NetworkObject ballObject, int botIndex)
    {
        PlayerRef = PlayerRef.None;
        CarObject = carObject;
        BallObject = ballObject;
        BotIndex = botIndex;
    }
}
