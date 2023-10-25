using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameraManager : NetworkBehaviour
{
    private Vector3 preGameOffset = new Vector3(0, 2, -2);

    public void ActivatePregamePerspecitve()
    {
        StartCoroutine(LockCameraToPlayer(preGameOffset));
    }

    //Sets the Camera's position to player position with given offset, and locks the view to the player position
    public IEnumerator LockCameraToPlayer(Vector3 offset)
    {
        Runner.TryGetPlayerObject(Runner.LocalPlayer, out NetworkObject networkObject);
        while (networkObject == null)
        {
            yield return new WaitForSeconds(1);
        }
        Transform playerTransform = networkObject.gameObject.transform;
        Vector3 relativeOffset = playerTransform.InverseTransformPoint(preGameOffset);
        Debug.Log(relativeOffset);

        //ArenaCamera.transform.position = newCameraPosition;
        //ArenaCamera.transform.LookAt(playerPosition);
    }
}
