using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDataScript : NetworkBehaviour
{
    public GameObject PlayerBall;
    [Networked] public bool isReady { get; set; } = false;
}
