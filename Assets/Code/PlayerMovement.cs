using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : NetworkBehaviour
{
    public KCC Kcc { get; private set; }
    [Networked] public NetworkBool IsTurningFast { get; set; } = false;

    public override void Spawned()
    {
        base.Spawned();
        Kcc = GetComponent<KCC>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<NetworkInputData>(out var input))
        {
           Rotate(input.JoystickBackgroundPosition, input.JoystickHandlePosition);
           Move(input.JoystickBackgroundPosition, input.JoystickHandlePosition);
           DetermineRotationSpeed(input);
        }
    }

    private void Rotate(Vector2 backgroundPos, Vector2 handlePos)
    {
        float rotationAngle = GetRotationAngle(backgroundPos, handlePos) + GetCurrentCarAngle();

        // Zero check to prevent rb to snap back to zero rotation.
        if (rotationAngle != 0)
        {
            Kcc.SetLookRotation(Quaternion.Lerp(Kcc.Data.TransformRotation, Quaternion.Euler(0, rotationAngle, 0), .05f));
            
        }
    }

    private void Move(Vector2 backgroundPos, Vector2 handlePos)
    {
        Kcc.SetInputDirection(GetMovementDirection(backgroundPos, handlePos));
    }

    private float GetRotationAngle(Vector2 initialVector, Vector2 lastVector)
    {
        Vector2 DifferenceVector = (lastVector - initialVector);
        if (DifferenceVector.magnitude > 0)
        {
            float differenceAngle = 90 - Mathf.Atan2(DifferenceVector.y, DifferenceVector.x) * Mathf.Rad2Deg;
            return differenceAngle;
        }
        else
        {
            return 0;
        }
        
    }

    private Vector3 GetMovementDirection(Vector2 backgroundPos, Vector2 handlePos)
    {
        Vector2 inputVector = handlePos - backgroundPos;
        if (inputVector.magnitude > 0)
        {
            return Quaternion.AngleAxis(GetCurrentCarAngle(), Vector3.up) * Vector3.right;
        }
        else
        {
            return Vector2.zero;
        }
    }

    private float GetCurrentCarAngle()
    {
        return Kcc.Data.TransformRotation.eulerAngles.y;
    }

    private void DetermineRotationSpeed(NetworkInputData input)
    {
        Vector2 inputVector = input.JoystickHandlePosition - input.JoystickBackgroundPosition;
        if (inputVector.y > 0)
        {
            RPC_SetBool(false);
        }
        else
        {
            RPC_SetBool(true);
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    public void RPC_SetBool(bool status)
    {
        IsTurningFast = status;
    }
}
