using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : NetworkBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "KCCCollider")
        {
            GameObject carObject = other.transform.parent.gameObject;
            carObject.SetActive(false);

            if (carObject.GetComponent<NetworkObject>().HasInputAuthority)
            {
                GameManager.CameraManager.DeathCamera.gameObject.SetActive(true);
                GameManager.UIManager.DeathScreen.SetActive(true);
            }
        }
    }
}
