using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotLogic : NetworkBehaviour
{
    public enum State {Idle, Chase, Attack}
    public Vector3 MovementDirection;
    public State CurrentState = State.Idle;
    private int _attackDistance = 15;
    private int _chaseDistance = 60;

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (!Runner.IsServer) return;

        switch (CurrentState)
        {
            case State.Idle:
                break;

            case State.Chase:
                SetMovementDirection();
                DetermineState();
                break;

            case State.Attack:
                DetermineState();
                break;
        }
    }

    private void DetermineState()
    {

        if ((GetDistanceVector().magnitude != 0) && (GetDistanceVector().magnitude < _attackDistance))
        {
            CurrentState = State.Attack;
        }
        else if(GetDistanceVector().magnitude < _chaseDistance)
        {
            CurrentState = State.Chase;
        }
    }

    private void SetMovementDirection()
    {
        MovementDirection = GetDistanceVector().normalized;
    }

    private Vector3 GetDistanceVector()
    {
        return FindClosestCarPosition() - transform.position;
    }

    private Vector3 FindClosestCarPosition()
    {
        if (Runner.IsServer)
        {
            Vector3 targetPosition = transform.position;
            float minDistance = _chaseDistance;

            foreach (CharacterData characterData in Spawner.ActiveCharacters)
            {
                Vector3 carPosition = characterData.CarObject.transform.position;
                float distance = (transform.position - carPosition).magnitude;

                if ((distance < minDistance) && (distance != 0))
                {
                    targetPosition = carPosition;
                    minDistance = distance;
                }
            }
            return targetPosition;
        }
        return transform.position;
    }
}
