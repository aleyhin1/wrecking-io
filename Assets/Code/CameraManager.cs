using Fusion;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : NetworkBehaviour
{ 
    public Camera MenuCamera;
    public Camera DeathCamera;

    public void ActivateDeathCamera()
    {
        DeathCamera.gameObject.SetActive(true);
    }
}
