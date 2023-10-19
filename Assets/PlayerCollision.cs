using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : NetworkBehaviour
{
    private KCC _playerKCC;

    public override void Spawned()
    {
        base.Spawned();
        _playerKCC = GetComponent<KCC>();
        _playerKCC.OnCollisionEnter += BallCollision;
    }

    private void BallCollision(KCC kcc, KCCCollision collision)
    {
        if (collision.Collider.tag == "Ball")
        {
            Vector3 ballVelocity = collision.Collider.gameObject.GetComponent<BallMovement>().pseudoVelocity;
            kcc.AddExternalVelocity(ballVelocity);
        }
    }
}
