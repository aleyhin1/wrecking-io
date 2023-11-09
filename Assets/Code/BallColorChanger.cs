using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallColorChanger : NetworkBehaviour
{
    public static BallColorChanger Instance { get; private set; }
    public GameObject[] BallModels;

    private GameObject _activeBallModel;


    public override void Spawned()
    {
        base.Spawned();

        if (Object.HasInputAuthority)
        {
            Instance = this;
        }
        _activeBallModel = BallModels[0];
    }

    [Rpc(RpcSources.All, RpcTargets.All)]
    public void RPC_ChangeBallModel(int index)
    {
        ChangeBallModel(index);
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SetBallColorData(PlayerRef player, int ballColorIndex)
    {
        CharacterData targetCharacterData = Spawner.ActiveCharacters.Find(x => x.PlayerRef == player);
        targetCharacterData.BallColorIndex = ballColorIndex;
    }

    public void ChangeBallModel(int index)
    {
        _activeBallModel.SetActive(false);
        BallModels[index].gameObject.SetActive(true);
        _activeBallModel = BallModels[index];
    }

    public void SetBallColorData(int ballColorIndex)
    {
        RPC_SetBallColorData(Runner.LocalPlayer, ballColorIndex);
    }

}
