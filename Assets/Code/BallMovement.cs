using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BallMovement : NetworkBehaviour
{
    [Networked] public PlayerRef TargetPlayer { get; set; }
    public Vector3 pseudoVelocity;
    private KCC _carKcc;
    private KCC _ballKcc;
    private Vector3 _lastFramePosition;

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
            
            SetPseudoVelocity();
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
        Vector3 position = playerPosition + GetOffsetWorldVector(8, rotation);
        Vector3 ropeForce = (position - _ballKcc.Data.TargetPosition) * 75;
        _ballKcc.SetExternalForce(ropeForce);
    }

    private void SetBallRotation()
    {
        Quaternion rotation = _carKcc.Data.LookRotation;
        _ballKcc.SetLookRotation(rotation);
    }

    private void SetPseudoVelocity()
    {
        if (_lastFramePosition != null)
        {
            pseudoVelocity = (transform.position - _lastFramePosition) / Runner.DeltaTime;
        }
        _lastFramePosition = transform.position;
    }

    public Vector3 GetOffsetWorldVector(float distance, Quaternion rotation)
    {
        return new Vector3(-distance * Mathf.Cos(rotation.eulerAngles.y * Mathf.Deg2Rad), 0, distance * Mathf.Sin(rotation.eulerAngles.y * Mathf.Deg2Rad));
    }

    public void BindCarKcc()
    {
        NetworkObject targetCarObject = Runner.GetPlayerObject(TargetPlayer);
        KCC targetKcc = targetCarObject.GetComponent<KCC>();
        _carKcc = targetKcc;
    }
}