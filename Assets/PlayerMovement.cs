using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    public Camera _camera;
    private NetworkRigidbody _networkRigidbody;
    private Rigidbody _rigidBody;

    [SerializeField] private float _movementSpeed = 20;
    //private NetworkCharacterControllerPrototype _characterController;

    public override void Spawned()
    {
        base.Spawned();
        _networkRigidbody = GetComponent<NetworkRigidbody>();
        _rigidBody = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (GetInput<NetworkInputData>(out var input))
        {
            Rotate(input.JoystickBackgroundPosition, input.JoystickHandlePosition);
           Move(input.JoystickBackgroundPosition, input.JoystickHandlePosition);
            //_networkRigidbody.Render();
        }
    }

    private void Rotate(Vector2 backgroundPos, Vector2 handlePos)
    {
        float rotationAngle = GetRotationAngle(backgroundPos, handlePos);

        _rigidBody.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), .1f);
    }

    private void Move(Vector2 backgroundPos, Vector2 handlePos)
    {
        //transform.Translate(GetMovementDirection(backgroundPos, handlePos) * _movementSpeed * Runner.DeltaTime, Space.World);
        //_rigidBody.MovePosition(GetMovementDirection(backgroundPos, handlePos) * _movementSpeed * Runner.DeltaTime);
        _rigidBody.position += GetMovementDirection(backgroundPos, handlePos) * _movementSpeed * Runner.DeltaTime;
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
