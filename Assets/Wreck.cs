using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wreck : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.rigidbody != null)
        {
            collision.rigidbody.AddForceAtPosition(collision.relativeVelocity * 10, collision.GetContact(0).point, ForceMode.Impulse);
            Debug.Log(collision.relativeVelocity);
        }
    }
}
