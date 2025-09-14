using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    private GameState currentState;
    private bool isTransitioning;
    private bool escHandledThisFrame = false;

    public event Action<GameState> OnStateChanged;

    [Header("References")]
    [SerializeField] private LevelUpController levelUpController;

    private List<IPausable> pausableSystems = new List<IPausable>();
    private List<IGameplaySystem> gameplaySystems = new List<IGameplaySystem>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        InitializeStates();
        FindAllSystems();

        if (levelUpController == null)
        {
            levelUpController = FindObjectOfType<LevelUpController>();
        }
    }

    private void FindAllSystems()
    {
        MonoBehaviour[] allMonoBehaviours = FindObjectsOfType<MonoBehaviour>(true);

        pausableSystems.Clear();
        gameplaySystems.Clear();

        foreach (var mono in allMonoBehaviours)
        {
            if (mono == null) continue;

            if (mono is IPausable pausable)
            {
                if (!pausableSystems.Contains(pausable))
                {
                    pausableSystems.Add(pausable);
                }
            }

            if (mono is IGameplaySystem gameplaySystem)
            {
                if (!gameplaySystems.Contains(gameplaySystem))
                {
                    gameplaySystems.Add(gameplaySystem);
                }
            }
        }
    }

    private void CleanDestroyedSystems()
    {
        pausableSystems.RemoveAll(system => system == null || system.Equals(null));
        gameplaySystems.RemoveAll(system => system == null || system.Equals(null));
    }

    public void SetPauseForAllSystems(bool paused)
    {
        CleanDestroyedSystems();
        foreach (var system in pausableSystems)
        {
            if (system != null && !system.Equals(null))
            {
                system.SetPaused(paused);
            }
        }
    }

    public void SetPauseForGameplaySystems(bool paused)
    {
        CleanDestroyedSystems();
        foreach (var system in gameplaySystems)
        {
            if (system != null && !system.Equals(null))
            {
                system.SetPaused(paused);
            }
        }
    }

    private void InitializeStates()
    {
        SwitchState(new GameplayState());
    }

    public void SwitchState(GameState newState)
    {
        if (isTransitioning || currentState?.GetType() == newState.GetType())
            return;

        StartCoroutine(TransitionToState(newState));
    }

    private IEnumerator TransitionToState(GameState newState)
    {
        isTransitioning = true;
        escHandledThisFrame = true; // Блокируем ESC во время перехода

        currentState?.ExitState();

        newState.Initialize(this);
        newState.EnterState();

        currentState = newState;
        OnStateChanged?.Invoke(currentState);

        isTransitioning = false;
        yield return null;

        escHandledThisFrame = false;
    }

    public LevelUpController GetLevelUpController() => levelUpController;

    private void Update()
    {
        currentState?.Update();

        if (Input.GetKeyDown(KeyCode.Escape) && !escHandledThisFrame)
        {
            escHandledThisFrame = true;

            if (isTransitioning)
            {
                return;
            }

            if (currentState?.CanPauseWithESC == true)
            {
                if (currentState is GameplayState)
                {
                    currentState.RequestPause();
                }
                else if (currentState is PausedState)
                {
                    currentState.RequestResume();
                }
            }
        }
    }

    public void RequestPause() => currentState?.RequestPause();
    public void RequestResume() => currentState?.RequestResume();
    public void RequestLevelUp() => currentState?.RequestLevelUp();

    public bool IsCurrentState<T>() where T : GameState => currentState is T;
}