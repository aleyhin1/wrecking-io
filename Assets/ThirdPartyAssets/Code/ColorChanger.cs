using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : NetworkBehaviour
{
    public static ColorChanger Instance {  get; private set; }
    public MeshRenderer[] CarRenderers;
    public Material[] CarMaterials;


    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            Instance = this;
        }
    }


    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangeCarColor(int index)
    {
        foreach (var meshRenderer in CarRenderers)
        {
            meshRenderer.material = CarMaterials[index];
        }
    }
}