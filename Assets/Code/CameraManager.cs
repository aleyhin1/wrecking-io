using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{ 
    public Camera MenuCamera;
    public Camera ArenaCamera;

    public void SwitchCameraToArena()
    {
        MenuCamera.gameObject.SetActive(false);
        ArenaCamera.gameObject.SetActive(true);
    }
}
