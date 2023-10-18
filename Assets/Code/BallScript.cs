using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : NetworkBehaviour
{
    private Rigidbody _rigidbody;
    private NetworkRigidbody _networkRigidbody;

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
    }

    private Vector3 GetOffsetWorldVector(float distance, Quaternion rotation)
    {
        return new Vector3(-distance * Mathf.Cos(rotation.eulerAngles.y * Mathf.Deg2Rad), 0, distance * Mathf.Sin(rotation.eulerAngles.y * Mathf.Deg2Rad));
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.rigidbody != null)
    //    {
    //        Vector3 velocity = _rigidBody.velocity;
    //        if (velocity != Vector3.zero)
    //        {
    //            collision.rigidbody.AddForceAtPosition(velocity, collision.GetContact(0).point, ForceMode.Impulse);
    //            Debug.Log(collision.relativeVelocity);
    //        }

    //    }
    //}
}