using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGameScript : NetworkBehaviour
{
    public Spawner Spawner;
    public Transform[] SpawnPositions;
    public Transform CenterPoint;

    private Quaternion FindRotation(int index)
    {
        Vector3 DifferenceVector = CenterPoint.position - SpawnPositions[index].position;
        Quaternion rotation = Quaternion.LookRotation(DifferenceVector, Vector3.up);
        return rotation;
    }

    [Rpc(RpcSources.StateAuthority, RpcTargets.All)]
    public void RPC_StartGame()
    {
        int index = 0;
        foreach (NetworkObject networkObject in Spawner._spawnedCharacters.Values)
        {
            Quaternion currentRotation = networkObject.gameObject.transform.rotation;
            networkObject.gameObject.GetComponent<NetworkRigidbody>().TeleportToPositionRotation(SpawnPositions[index].position, FindRotation(index) * currentRotation);
            index++;
        }
    }
}
