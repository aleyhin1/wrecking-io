using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallMovement : NetworkBehaviour
{
    public KCC _carKcc { get; set; }
    private KCC _ballKcc;

    public override void Spawned()
    {
        base.Spawned();
        _ballKcc = GetComponent<KCC>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_carKcc != null)
        {
            SetBallVelocity();
            SetRopeForce();
            SetBallRotation();
        }
    }

    // Ball's velocity from moving car
    private void SetBallVelocity()
    {
        _ballKcc.SetExternalVelocity(_carKcc.Data.DesiredVelocity * .075f);
    }

    // Force applied to the ball from rope
    private void SetRopeForce()
    {
        Vector3 playerPosition = _carKcc.Data.TargetPosition;
        Quaternion rotation = _carKcc.Data.LookRotation;
        Vector3 position = playerPosition + TransformUtils.GetOffsetWorldVector(8, rotation);
        Vector3 ropeForce = (position - _ballKcc.Data.TargetPosition) * 75;
        _ballKcc.SetExternalForce(ropeForce);
    }

    private void SetBallRotation()
    {
        Vector3 carPosition = _carKcc.Data.TargetPosition;
        Vector3 ballPosition = _ballKcc.Data.TargetPosition;
        Vector3 directionVector = (carPosition - ballPosition).normalized;
        float angle = Vector3.SignedAngle(Vector3.right, directionVector, Vector3.up);

        Vector3 rotationVector = new Vector3(0, angle, 0);
        Quaternion rotation = Quaternion.Euler(rotationVector);
        _ballKcc.SetLookRotation(rotation);
    }
}