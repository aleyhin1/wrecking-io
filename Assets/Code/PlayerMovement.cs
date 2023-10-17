using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public static PlayerMovement Instance { get; private set; }

    private KCC _kcc;

    [SerializeField] private float _movementSpeed = 20;
    private Rigidbody _rigidBody;

    public override void Spawned()
    {
        base.Spawned();
        Instance = this;
        _rigidBody = GetComponent<Rigidbody>();
        _kcc = GetComponent<KCC>();
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
        float rotationAngle = GetRotationAngle(backgroundPos, handlePos);

        // Zero check to prevent rb to snap back to zero rotation.
        if (rotationAngle != 0)
            _rigidBody.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), .1f);
    }

    private void Move(Vector2 backgroundPos, Vector2 handlePos)
    {
        //_rigidBody.position += GetMovementDirection(backgroundPos, handlePos) * _movementSpeed * Runner.DeltaTime;
        _kcc.SetInputDirection(GetMovementDirection(backgroundPos, handlePos));
    }

    private float GetRotationAngle(Vector2 initialVector, Vector2 lastVector)
    {
        Vector2 DifferenceVector = (lastVector - initialVector);
        return Mathf.Atan2(-DifferenceVector.y, DifferenceVector.x) * Mathf.Rad2Deg;
    }

    private Vector3 GetMovementDirection(Vector2 initialVector, Vector2 lastVector)
    {
        Vector2 DifferenceVectorNormalized = (lastVector - initialVector).normalized;
        Vector3 MovementDirection = new Vector3(DifferenceVectorNormalized.x, 0 ,DifferenceVectorNormalized.y);
        
        return MovementDirection;
    }
}
