using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct GameSettings : INetworkStruct
{
    public const byte MIN_BOTCOUNT = 0;
    public const byte MAX_BOTCOUNT = 4;

    [Range(MIN_BOTCOUNT, MAX_BOTCOUNT)]
    public int BotCount;

    public static GameSettings Default => new GameSettings
    {
        BotCount = 0,
    };
}
