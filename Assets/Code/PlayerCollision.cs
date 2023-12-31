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
            BallMovement ballMovementScript = collision.Collider.GetComponent<BallMovement>();
            GameObject targetCarOfBall = ballMovementScript._carKcc.gameObject;
            
            // Only collide with balls other than your own
            if (targetCarOfBall != this.gameObject)
            {
                Vector3 ballVelocity = collision.Collider.gameObject.GetComponent<KCC>().Data.DesiredVelocity * 1.2f;
                kcc.AddExternalImpulse(ballVelocity);
                targetCarOfBall.GetComponent<KCC>().AddExternalImpulse(-ballVelocity);
            }
        }
    }
}
