using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera MenuCamera;
    public Camera PreRoomCamera;
    public Camera ArenaCamera;

    public void SwitchCamera(Camera previousCamera, Camera newCamera)
    {
        previousCamera.gameObject.SetActive(false);
        newCamera.gameObject.SetActive(true);
    }
}
