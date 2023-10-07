using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsPanelScript : MonoBehaviour
{
    public Spawner Spawner;
    public Transform[] SpawnPositions;
    public Transform CenterPoint;

    public void StartGame()
    {
        int index = 0;
        foreach (NetworkObject networkObject in Spawner._spawnedCharacters.Values)
        {
            networkObject.gameObject.transform.position = SpawnPositions[index].position;
            Quaternion currentRotation = networkObject.gameObject.transform.rotation;
            networkObject.gameObject.transform.rotation = FindRotation(index) * currentRotation;
            index++;
        }
    }

    private Quaternion FindRotation(int index)
    {
        Vector3 DifferenceVector = CenterPoint.position - SpawnPositions[index].position;
        Quaternion rotation = Quaternion.LookRotation(DifferenceVector, Vector3.up);
        return rotation;
    }
}
