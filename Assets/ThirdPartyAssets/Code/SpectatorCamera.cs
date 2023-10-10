using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpectatorCamera : NetworkBehaviour
{
    private void Update()
    {
        var activePlayer = Runner.ActivePlayers.GetEnumerator().Current;
        LockCameraToPlayer(activePlayer);
    }

    private void LockCameraToPlayer(PlayerRef playerRef)
    {
        Vector3 playerPosition = Runner.GetPlayerObject(playerRef).gameObject.transform.position;
        float Offset = -12.8f;
        Vector3 movePosition = new Vector3(playerPosition.x, 40, playerPosition.z + Offset);
        transform.position = movePosition;
    }
}
