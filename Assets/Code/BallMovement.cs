using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    public Vector3 pseudoVelocity;
    private Vector3 lastFramePosition;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public override void FixedUpdateNetwork()
    {
        Vector3 playerPosition = PlayerMovement.Instance.Kcc.Data.BasePosition;
        Quaternion rotation = PlayerMovement.Instance.Kcc.Data.LookRotation;
        Vector3 position = playerPosition + GetOffsetWorldVector(10, rotation);

        if (_rigidbody != null)
        {
            _rigidbody.Move(Vector3.Lerp(transform.position, position, .2f), Quaternion.Lerp(transform.rotation, rotation, .2f));
        }
        SetPseudoVelocity();
    }

    private void SetPseudoVelocity()
    {
        if (lastFramePosition != null)
        {
            pseudoVelocity = (transform.position - lastFramePosition) / Runner.DeltaTime;
        }
        lastFramePosition = transform.position;
    }

    private Vector3 GetOffsetWorldVector(float distance, Quaternion rotation)
    {
        return new Vector3(-distance * Mathf.Cos(rotation.eulerAngles.y * Mathf.Deg2Rad), 0, distance * Mathf.Sin(rotation.eulerAngles.y * Mathf.Deg2Rad));
    }
}