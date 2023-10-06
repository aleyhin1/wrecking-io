using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : NetworkBehaviour
{
    public MeshRenderer[] CarRenderers;
    public Material[] CarMaterials;


    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangeCarColor(int index)
    {
        foreach (var c in CarRenderers)
        {
            c.material = CarMaterials[index];
        }
    }
}