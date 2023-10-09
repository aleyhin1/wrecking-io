using Fusion;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    public enum EGameState { Pregame, Play, Death, Postgame}

    [Networked] public EGameState Previous { get; set; }
    [Networked] public EGameState Current { get; set; }

    protected StateMachine<EGameState> StateMachine = new StateMachine<EGameState>();

    public override void Spawned()
    {
        StateMachine[EGameState.Pregame].onEnter = state =>
        {

        };

        StateMachine[EGameState.Play].onEnter = state =>
        {
            if (Runner.IsServer)
            {
                GameManager.Instance.PreGameScreen.SetActive(false);
                GameManager.StartGameScript.RPC_StartGame();
                GameManager.ArenaManager.enabled = true;
                GameManager.Instance.PreRoomCube.SetActive(false);
            }
            if (Runner.IsClient)
            {
                GameManager.Instance.PreGameScreen.SetActive(false);
                GameManager.CameraManager.SwitchCamera(GameManager.CameraManager.PreRoomCamera, GameManager.CameraManager.ArenaCamera);
                GameManager.ArenaManager.enabled = true;
                GameManager.Instance.PreRoomCube.SetActive(false);
            }
        };

        StateMachine[EGameState.Death].onEnter = state =>
        {
            //GameManager.CameraManager.SwitchCamera(GameManager.CameraManager.ArenaCamera, GameManager.CameraManager.SpectatorCamera);
            GameManager.Instance.DeathScreen.SetActive(true);
        };
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsForward)
            StateMachine.Update(Current, Previous);
    }

    public void Server_SetState(EGameState st)
    {
        if (Current == st) return;
        Previous = Current;
        Current = st;
    }
}
