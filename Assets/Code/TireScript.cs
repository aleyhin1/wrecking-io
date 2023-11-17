using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TireScript : NetworkBehaviour
{
    [SerializeField] private TrailRenderer _tireTrail0;
    [SerializeField] private TrailRenderer _tireTrail1;
    [SerializeField] private TrailRenderer _tireTrail2;
    [SerializeField] private TrailRenderer _tireTrail3;
    private PlayerMovement _playerMovement;

    public override void Spawned()
    {
        base.Spawned();
        _playerMovement = GetComponentInParent<PlayerMovement>();
    }

    public override void FixedUpdateNetwork()
    {
        base.FixedUpdateNetwork();

        if (_playerMovement != null)
        {
            if (_playerMovement.IsTurningFast)
            {
                SetEmittingToTrails(true);
            }
            else
            {
                SetEmittingToTrails(false);
            }
        }
        else
        {
            _playerMovement = GetComponentInParent<PlayerMovement>();
        }
    }

    private void SetEmittingToTrails(bool status)
    {
        _tireTrail0.emitting = status;
        _tireTrail1.emitting = status;
        _tireTrail2.emitting = status;
        _tireTrail3.emitting = status;
    }
}
