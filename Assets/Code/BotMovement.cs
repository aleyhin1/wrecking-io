using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotMovement : NetworkBehaviour
{
    public KCC Kcc { get; private set; }
    private BotLogic _botLogic;
    private const float turnSpeed = .1f;
    
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
            Kcc.SetLookRotation(Quaternion.Lerp(transform.rotation, GetRotationQuaternion(), turnSpeed));
        }
    }

    private Quaternion GetRotationQuaternion()
    {
        Quaternion rotationQuaternion = Quaternion.Euler(0, FindRotationAngle(), 0);
        return rotationQuaternion;
    }

    private float FindRotationAngle()
    {
        Vector3 directionToLook = _botLogic.MovementDirection.normalized;
        float rotationAngle = Mathf.Atan2(-directionToLook.z, directionToLook.x) * Mathf.Rad2Deg;
        
        return rotationAngle;
    }
}
