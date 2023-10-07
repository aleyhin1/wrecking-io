using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstraint : MonoBehaviour
{
    private void Update()
    {
        LockCameraToPlayer();
    }

    private void LockCameraToPlayer()
    {
        Vector3 playerPosition = ColorChanger.instance.gameObject.transform.position;
        Vector3 movePosition = new Vector3(playerPosition.x, 10, playerPosition.z);
        transform.position = movePosition;
    }
}