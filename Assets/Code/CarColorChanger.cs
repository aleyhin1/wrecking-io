using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarColorChanger : NetworkBehaviour
{
    public static CarColorChanger Instance {  get; private set; }
    public MeshRenderer[] CarRenderers;
    public Material[] CarMaterials;
    public SkinnedMeshRenderer PlayerRenderer;
    public Material[] PlayerMaterials;


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

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangePlayerColor(int index)
    {
        PlayerRenderer.SetMaterials(new List<Material> {PlayerMaterials[index], PlayerMaterials[index]});
    }
}