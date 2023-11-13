using Fusion;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

public class GamerTagScript : NetworkBehaviour
{
    [Networked(OnChanged = nameof(SetGamerTag))] public string GamerTag { get; set; }
    [SerializeField]
    private TextMeshProUGUI _textMesh;

    public override void Spawned()
    {
        GetGamerTag();
    }

    private void Start()
    {
        SetRandomColorToText();
        SetGamerTag();
    }

    private void Update()
    {
        LookAtCamera();
    }

    public void LookAtCamera()
    {
        Camera activeCamera = Camera.allCameras[0];
        transform.LookAt(activeCamera.transform.position);
    }

    private void GetGamerTag()
    {
        if (HasInputAuthority)
        {
            RPC_SendGamerTag(GameManager.Instance.NickName);
        }
    }

    public static void SetGamerTag(Changed<GamerTagScript> changed)
    {
        changed.Behaviour.SetGamerTag();
    }

    private void SetGamerTag()
    {
        _textMesh.text = GamerTag;
    }

    [Rpc(RpcSources.InputAuthority, RpcTargets.StateAuthority)]
    private void RPC_SendGamerTag(string gamerTag)
    {
        GamerTag = gamerTag;
    }

    private void SetRandomColorToText()
    {
        _textMesh.color = Random.ColorHSV(0, 1, .75f, 1, .75f, 1);
    }
}
