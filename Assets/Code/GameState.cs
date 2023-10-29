using Fusion;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameState : NetworkBehaviour
{
    public enum EGameState {Off, Pregame, Play, Death, Postgame}
    [Networked] public EGameState Previous { get; set; } = EGameState.Off;
    [Networked] public EGameState Current { get; set; }

    protected StateMachine<EGameState> StateMachine = new StateMachine<EGameState>();

    public override void Spawned()
    {

        StateMachine[EGameState.Pregame].onEnter = state =>
        {
            GameManager.CameraManager.MenuCamera.gameObject.SetActive(false);
            GameManager.UIManager.MainMenuScreen.gameObject.SetActive(false);
            GameManager.UIManager.PreGameScreen.gameObject.SetActive(true);

            if (Runner.IsServer)
            {
                GameManager.UIManager.SettingsPanel.gameObject.SetActive(true);
            }

            Previous = EGameState.Pregame;
        };

        StateMachine[EGameState.Pregame].onUpdate = state =>
        {
            if (HasStateAuthority)
            {
                int readyPlayers = GameManager.Instance.ReadyPlayerCount;
                int activePlayers = Runner.ActivePlayers.Count<PlayerRef>();
                if (readyPlayers == activePlayers - 1)
                {
                    GameManager.UIManager.StartToggle.interactable = true;
                }
                else
                {
                    GameManager.UIManager.StartToggle.interactable = false;
                }
            }
        };

        StateMachine[EGameState.Play].onEnter = state =>
        {
            GameManager.Instance.PreGameScreen.SetActive(false);
            GameManager.ArenaManager.enabled = true;
            GameManager.Instance.Joystick.SetActive(true);
        };

        StateMachine[EGameState.Death].onEnter = state =>
        {
            GameManager.Instance.DeathScreen.SetActive(true);
        };
    }

    public override void FixedUpdateNetwork()
    {
        if (Runner.IsForward)
        {
            StateMachine.Update(Current, Previous);
        } 
    }

    public void Server_SetState(EGameState st)
    {
        if (Current == st)
        {
            return;
        }

        Previous = Current;
        Current = st;
    }
}
