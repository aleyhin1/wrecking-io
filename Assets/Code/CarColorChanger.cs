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
        ChangeCarColor(index);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangePlayerColor(int index)
    {
        ChangePlayerColor(index);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetCarColorData(PlayerRef player, int carColorIndex)
    {
        CharacterData targetCharacterData = Spawner.ActiveCharacters.Find(x => x.PlayerRef == player);
        targetCharacterData.CarColorIndex = carColorIndex;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetPlayerColorData(PlayerRef player, int playerColorIndex)
    {
        CharacterData targetCharacterData = Spawner.ActiveCharacters.Find(x => x.PlayerRef == player);
        targetCharacterData.PlayerColorIndex = playerColorIndex;
    }

    public void ChangeCarColor(int index)
    {
        foreach (var meshRenderer in CarRenderers)
        {
            meshRenderer.material = CarMaterials[index];
        }
    }

    public void ChangePlayerColor(int index)
    {
        PlayerRenderer.SetMaterials(new List<Material> { PlayerMaterials[index], PlayerMaterials[index] });
    }

    public void SetPlayerColorData(int playerColorIndex)
    {
        RPC_SetPlayerColorData(Runner.LocalPlayer, playerColorIndex);
    }

    public void SetCarColorData(int carColorIndex)
    {
        RPC_SetCarColorData(Runner.LocalPlayer, carColorIndex);
    }
}