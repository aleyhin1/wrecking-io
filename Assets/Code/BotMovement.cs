using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : NetworkBehaviour
{
    public KCC Kcc { get; private set; }
    private BotLogic _botLogic;
    
    public override void Spawned()
    {
        base.Spawned();
        Kcc = GetComponent<KCC>();
        _botLogic = GetComponent<BotLogic>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();
        if (_botLogic.CurrentState == BotLogic.State.Chase)
        {
            Kcc.SetInputDirection(_botLogic.MovementDirection);
        }
    }
}
