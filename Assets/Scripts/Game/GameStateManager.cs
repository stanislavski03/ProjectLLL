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

    public void StartTransition(float duration)
    {
        Instance.IsTransitioning = true;
        // Можно добавить событие для начала перехода если нужно
    }

    public void EndTransition()
    {
        Instance.IsTransitioning = false;
        // Можно добавить событие для окончания перехода если нужно
    }
}