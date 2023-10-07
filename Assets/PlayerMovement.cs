using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public Camera _camera;

    public override void FixedUpdateNetwork()
    {
        if (GetInput<NetworkInputData>(out var input))
        {
            Rotate(input.JoystickBackgroundPosition, input.JoystickHandlePosition);
        }
    }

    private void Rotate(Vector2 backgroundPos, Vector2 handlePos)
    {
        float rotationAngle = GetRotationAngle(backgroundPos, handlePos);
        transform.eulerAngles = new Vector3 (0, rotationAngle, 0);
    }

    private float GetRotationAngle(Vector2 initialVector, Vector2 lastVector)
    {
        Vector2 DifferenceVectorNormalized = (lastVector - initialVector).normalized;
        return Mathf.Atan2(-DifferenceVectorNormalized.y, DifferenceVectorNormalized.x) * Mathf.Rad2Deg;
    }
}
