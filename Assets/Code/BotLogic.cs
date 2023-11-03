using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotLogic : NetworkBehaviour
{
    public enum State {Idle, Chase, Attack}
    public Vector3 MovementDirection;
    public State CurrentState = State.Idle;
    public LayerMask CarLayer;
    private List<Ray> _rays = new List<Ray>();

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        switch (CurrentState)
        {
            case State.Idle:
                break;

            case State.Chase:
                DrawRays();
                SetMovementDirection();
                break;

            case State.Attack:
                break;
        }
    }

    private void DrawRays()
    {
        _rays.Clear();

        Vector3 rayOffset = new Vector3(0, 2, 0);

        Ray rayWest = new Ray(transform.position + rayOffset, new Vector3(-1,0,0));
        Ray raySouthWest = new Ray(transform.position + rayOffset, new Vector3(-1, 0, -1));
        Ray raySouth = new Ray (transform.position + rayOffset, new Vector3(0, 0, -1));
        Ray raySouthEast = new Ray(transform.position + rayOffset, new Vector3(1, 0, -1));
        Ray rayEast = new Ray(transform.position + rayOffset, new Vector3(1, 0, 0));
        Ray rayNorthEast = new Ray(transform.position + rayOffset, new Vector3(1, 0, 1));
        Ray rayNorth = new Ray(transform.position + rayOffset, new Vector3(0,0,1));
        Ray rayNorthWest = new Ray(transform.position + rayOffset, new Vector3(-1, 0, 1));

        _rays.Add(rayWest);
        _rays.Add(raySouthWest);
        _rays.Add(raySouth);
        _rays.Add(raySouthEast);
        _rays.Add(rayEast);
        _rays.Add(rayNorthEast);
        _rays.Add(rayNorth);
        _rays.Add(rayNorthWest);
    }

    private void SetMovementDirection()
    {
        MovementDirection = FindClosestCarPosition() - transform.position;
    }

    private Vector3 FindClosestCarPosition()
    {
        Vector3 targetPosition = Vector3.zero;

        float minDistance = 15;

        foreach (Ray ray in _rays)
        {
            Debug.DrawRay(ray.origin, ray.direction * 15, Color.white, .2f);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, 15, CarLayer))
            {
                Debug.DrawRay(ray.origin, ray.direction * 15, Color.red, .2f);
                if (raycastHit.collider.name == "KCCCollider")
                {
                    Vector3 collidingCarPosition = raycastHit.collider.GetComponentInParent<Transform>().position;

                    float distance = (transform.position - collidingCarPosition).magnitude;

                    if (distance < minDistance)
                    {
                        targetPosition = raycastHit.transform.position;
                        minDistance = distance;
                    }
                }
            }
        }
        return targetPosition;
    }
}
