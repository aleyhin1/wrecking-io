using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wreck : MonoBehaviour
{
    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            Vector3 velocity = _rigidBody.velocity;
            if (velocity != Vector3.zero)
            {
                collision.rigidbody.AddForceAtPosition(velocity, collision.GetContact(0).point, ForceMode.Impulse);
                Debug.Log(collision.relativeVelocity);
            }
            
        }
    }
}
