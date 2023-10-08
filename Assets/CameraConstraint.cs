using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConstraint : MonoBehaviour
{
    public GameObject JoyStick;

    private void OnEnable()
    {
        JoyStick.SetActive(true);
    }

    private void Update()
    {
        LockCameraToPlayer();
    }

    private void LockCameraToPlayer()
    {
        Vector3 playerPosition = ColorChanger.instance.gameObject.transform.position;
        float Offset = -12.8f;
        Vector3 movePosition = new Vector3(playerPosition.x, 40, playerPosition.z + Offset);
        transform.position = movePosition;
    }
}