using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class GamerTagScript : MonoBehaviour
{
    private void Update()
    {
        LookAtCamera();
    }

    public void LookAtCamera()
    {
        Camera activeCamera = Camera.allCameras[0];
        transform.LookAt(activeCamera.transform.position);
    }
}
