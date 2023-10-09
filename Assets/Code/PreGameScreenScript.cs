using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreGameScreenScript : MonoBehaviour
{
    public void StartGame()
    {
        GameManager.Instance.RPC_StartArena();
    }
}
