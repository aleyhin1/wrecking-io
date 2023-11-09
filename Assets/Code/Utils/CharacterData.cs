using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData
{
    public PlayerRef PlayerRef { get; private set; }
    public NetworkObject CarObject { get; private set; }
    public NetworkObject BallObject { get; private set; }
    public int BotIndex { get; private set; }
    public int CarColorIndex { get; set; }
    public int PlayerColorIndex { get; set; }
    public int BallColorIndex { get; set; }

    public CharacterData(PlayerRef playerRef, NetworkObject carObject, NetworkObject ballObject)
    {
        PlayerRef = playerRef;
        CarObject = carObject;  
        BallObject = ballObject;
    }

    public CharacterData(NetworkObject carObject, NetworkObject ballObject, int botIndex, int carColorIndex, int playerColorIndex, 
        int ballColorIndex)
    {
        PlayerRef = PlayerRef.None;
        CarObject = carObject;
        BallObject = ballObject;
        BotIndex = botIndex;
        CarColorIndex = carColorIndex;
        PlayerColorIndex = playerColorIndex;
        BallColorIndex = ballColorIndex;
    }
}
