using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "KCCCollider")
        {
            GameObject carObject = other.transform.parent.gameObject;
            carObject.SetActive(false);

            GameManager.CameraManager.ActivateDeathCamera();
            GameManager.UIManager.DeathScreen.SetActive(true);
        }
    }
}
