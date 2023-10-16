using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{ 
    public Camera MenuCamera;
    public Camera ArenaCamera;

    private Vector3 preGameOffset = new Vector3(0, 2, -2);


    public void SwitchCameraToArena()
    {
        MenuCamera.gameObject.SetActive(false);
        ArenaCamera.gameObject.SetActive(true);
    }

    public void ActivatePregamePerspecitve()
    {
        StartCoroutine(LockCameraToPlayer(preGameOffset));
    }

    //Sets the Camera's position to player position with given offset, and locks the view to the player position
    public IEnumerator LockCameraToPlayer(Vector3 offset)
    {
        while (PlayerMovement.Instance == null)
        {
            yield return new WaitForSeconds(1);
        }
        Transform playerTransform = PlayerMovement.Instance.gameObject.transform;
        Vector3 relativeOffset = playerTransform.InverseTransformPoint(preGameOffset);
        Debug.Log(relativeOffset);
        
        //ArenaCamera.transform.position = newCameraPosition;
        //ArenaCamera.transform.LookAt(playerPosition);
    }
}
