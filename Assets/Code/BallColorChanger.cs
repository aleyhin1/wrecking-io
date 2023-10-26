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
            _activeBallModel = BallModels[0];
        }
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
    public void RPC_ChangeBallModel(int index)
    {
        _activeBallModel.SetActive(false);
        BallModels[index].gameObject.SetActive(true);
        _activeBallModel = BallModels[index];
    }

}
