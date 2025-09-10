using System.Collections;
using UnityEngine;

public class GameStateManager
{
    private static GameStateManager _instance;
    public static GameStateManager Instance
    {
        get
        {
            if (_instance == null)
                _instance = new GameStateManager();

            return _instance;
        }
    }

    public GameState CurrentGameState { get; private set; }
    public bool IsTransitioning { get; private set; }

    public delegate void GameStateChangeHandler(GameState newGameState);
    public event GameStateChangeHandler OnGameStateChanged;

    private GameStateManager()
    {
        CurrentGameState = GameState.Gameplay;
    }

    public void SetState(GameState newGameState)
    {
        if (newGameState == CurrentGameState || IsTransitioning)
            return;

        CurrentGameState = newGameState;
        OnGameStateChanged?.Invoke(newGameState);
    }

    public void SetLevelUpPause()
    {
        if (CurrentGameState == GameState.LevelUpPaused || IsTransitioning)
            return;

        CurrentGameState = GameState.LevelUpPaused;
        OnGameStateChanged?.Invoke(GameState.LevelUpPaused);
    }

    public void ResumeFromLevelUpPause()
    {
        if (CurrentGameState != GameState.LevelUpPaused || IsTransitioning)
            return;

        CurrentGameState = GameState.Gameplay;
        OnGameStateChanged?.Invoke(GameState.Gameplay);
    }

    public void StartTransition(float duration)
    {
        Instance.IsTransitioning = true;
    }

    public void EndTransition()
    {
        Instance.IsTransitioning = false;
    }
}