using Fusion;
using Fusion.KCC;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeScript : NetworkBehaviour
{
    private LineRenderer _lineRenderer;

    public override void Spawned()
    {
        base.Spawned();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    public void BindRope(KCC carKcc, KCC ballKcc)
    {
        _lineRenderer.useWorldSpace = true;
        StartCoroutine(StretchRope(carKcc, ballKcc));
    }

    private IEnumerator StretchRope(KCC carKcc, KCC ballKcc)
    {
        while (true)
        {
            _lineRenderer.SetPosition(0, GetRopeStartPosition(ballKcc));
            _lineRenderer.SetPosition(1, GetRopeEndPosition(carKcc));

            yield return new WaitForEndOfFrame();
        }
    }

    private Vector3 GetRopeStartPosition(KCC ballKcc)
    {
        Vector3 ballPosition = ballKcc.Data.TargetPosition;
        Vector3 ballOffset = new Vector3(0, 2, 0);
        Vector3 ropeStartPosition = ballPosition + ballOffset + LagCompensationOffset(ballKcc);
        return ropeStartPosition;
    }

    private Vector3 GetRopeEndPosition(KCC carKcc)
    {
        Vector3 carPosition = carKcc.Data.TargetPosition;
        Vector3 carOffset = TransformUtils.GetOffsetWorldVector(2, carKcc.Data.LookRotation) + new Vector3(0, 1, 0);
        Vector3 ropeEndPosition = carPosition + carOffset;
        return ropeEndPosition;
    }

    private Vector3 LagCompensationOffset(KCC ballKcc)
    {
        return ballKcc.Data.DesiredVelocity * .025f;
    }
}