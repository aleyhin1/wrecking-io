using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : NetworkBehaviour
{
    public NetworkObject targetCarObject;
    public Vector3 pseudoVelocity;

    private Rigidbody _rigidbody;
    private Vector3 _lastFramePosition;
    private KCC _carKcc;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _carKcc = targetCarObject.GetComponent<KCC>();
    }

    public override void FixedUpdateNetwork()
    {
        if (_carKcc == null) return;

        Vector3 playerPosition = _carKcc.Data.BasePosition;
        Quaternion rotation = _carKcc.Data.LookRotation;
        Vector3 position = playerPosition + GetOffsetWorldVector(10, rotation);

        if (_rigidbody != null)
        {
            _rigidbody.Move(Vector3.Lerp(transform.position, position, .2f), Quaternion.Lerp(transform.rotation, rotation, .2f));
        }
        SetPseudoVelocity();
    }

    private void SetPseudoVelocity()
    {
        if (_lastFramePosition != null)
        {
            pseudoVelocity = (transform.position - _lastFramePosition) / Runner.DeltaTime;
        }
        _lastFramePosition = transform.position;
    }

    private Vector3 GetOffsetWorldVector(float distance, Quaternion rotation)
    {
        return new Vector3(-distance * Mathf.Cos(rotation.eulerAngles.y * Mathf.Deg2Rad), 0, distance * Mathf.Sin(rotation.eulerAngles.y * Mathf.Deg2Rad));
    }
}