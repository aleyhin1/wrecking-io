using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : NetworkBehaviour
{
    public KCC Kcc { get; private set; }
    
    public override void Spawned()
    {
        base.Spawned();
        Kcc = GetComponent<KCC>();
    }


    
}
