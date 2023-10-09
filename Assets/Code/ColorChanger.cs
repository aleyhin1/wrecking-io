using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : NetworkBehaviour
{
    public static ColorChanger instance {  get; private set; }

    public MeshRenderer[] CarRenderers;
    public Material[] CarMaterials;

    public override void Spawned()
    {
        base.Spawned();
        if (Object.HasInputAuthority)
        {
            instance = this;
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangeCarColor(int index)
    {
        foreach (var c in CarRenderers)
        {
            c.material = CarMaterials[index];
        }
    }
}