using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public KCC Kcc { get; private set; }

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

    private Vector3 GetMovementDirection(Vector2 initialVector, Vector2 lastVector)
    {
        Vector2 inputVector = (lastVector - initialVector);
        if (inputVector.magnitude > 100)
        {   
            Vector3 movementDirection = Quaternion.AngleAxis(GetCurrentCarAngle(), Vector3.up) * Vector3.right;
            return movementDirection;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private float GetCurrentCarAngle()
    {
        return Kcc.Data.TransformRotation.eulerAngles.y;
    }
}
