using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    public enum EGameState {Off, Pregame, Play, Death, Postgame}
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
            GameManager.CameraManager.SwitchCameraToArena();
            GameManager.Instance.PreGameScreen.SetActive(false);
            GameManager.ArenaManager.enabled = true;
        };

        StateMachine[EGameState.Death].onEnter = state =>
        {
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
