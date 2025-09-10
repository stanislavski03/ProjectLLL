using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseController : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameState currentGameState = GameStateManager.Instance.CurrentGameState;
            
            // ESC работает только между Gameplay и Paused
            if (currentGameState == GameState.Gameplay)
            {
                GameStateManager.Instance.SetState(GameState.Paused);
            }
            else if (currentGameState == GameState.Paused)
            {
                GameStateManager.Instance.SetState(GameState.Gameplay);
            }
            // LevelUpPaused игнорируем ESC
        }
    }
}