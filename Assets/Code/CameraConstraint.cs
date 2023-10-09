using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstraint : MonoBehaviour
{
    [SerializeField] private GameObject _joystick;


    private void OnEnable()
    {
        _joystick.SetActive(true);
    }


    private void Update()
    {
        LockCameraToPlayer();
    }

    private void LockCameraToPlayer()
    {
        Vector3 playerPosition = ColorChanger.Instance.gameObject.transform.position;
        float offset = -12.8f;
        Vector3 movePosition = new Vector3(playerPosition.x, 30, playerPosition.z + offset);
        transform.position = movePosition;
    }
}