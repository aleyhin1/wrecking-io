using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : NetworkBehaviour
{
    [Networked] public PlayerRef TargetPlayer { get; set; }
    public Vector3 pseudoVelocity;
    private KCC _carKcc;

    private Rigidbody _rigidbody;
    private Vector3 _lastFramePosition;

    public override void Spawned()
    {
        base.Spawned();
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_carKcc != null)
        {
            Vector3 playerPosition = _carKcc.Data.TargetPosition;
            Quaternion rotation = _carKcc.Data.LookRotation;
            Vector3 position = playerPosition + GetOffsetWorldVector(10, rotation);

            _rigidbody.Move(position, rotation);
            SetPseudoVelocity();
        }
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