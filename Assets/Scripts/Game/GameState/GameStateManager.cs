using System;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

    public bool IsPaused { get; private set; }
    public bool IsInCountdown { get; private set; }
    public PauseType CurrentPauseType { get; private set; }
    public event Action<bool, PauseType> OnPauseStateChanged;
    public GameObject MenuCanvas;
    public GameObject SettingsCanvas;

    public enum PauseType
    {
        None,
        EscPause,
        LevelUpPause
    }

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

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !IsInCountdown)
        {
            if (CurrentPauseType == PauseType.LevelUpPause) return;
            TogglePause();
        }
    }

    public void TogglePause()
    {
        if (IsPaused)
        {
            AudioManager.Instance.ResumeMusic();
            SimpleFog.Instance.SetFogSmooth(SimpleFog.Instance.defaultFogDensity);
            MenuCanvas.SetActive(false);
            SettingsCanvas.SetActive(false);
            ItemsPanel.Instance.gameObject.SetActive(true);
            SetPaused(false);

            CountdownController countdown = FindObjectOfType<CountdownController>();
            if (countdown != null)
            {
                countdown.StartCountdown();
            }
        }
        else
        {
            SetPaused(true, PauseType.EscPause);
            SimpleFog.Instance.SetFogSmooth(SimpleFog.Instance.pauseFogDensity);
            MenuCanvas.SetActive(true);
            QuestPanel.Instance.SetQuestsInfo();
        }
    }

    public void SetPaused(bool paused, PauseType pauseType = PauseType.EscPause)
    {
        if (IsPaused == paused && CurrentPauseType == pauseType) return;
        
        IsPaused = paused;
        CurrentPauseType = paused ? pauseType : PauseType.None;

        EnemySpawnManager.Instance.ChangeActiveEnemySpawn(!paused);

        AudioManager.Instance.StopAllSFX();
        
        // Управление музыкой
        if (paused && pauseType == PauseType.EscPause)
        {
            AudioManager.Instance.DuckMusic();
        }
        else if (!paused)
        {
            // Возобновляем музыку при снятии ЛЮБОЙ паузы
            AudioManager.Instance.UnduckMusic();
        }
        
        if (!IsInCountdown)
        {
            Time.timeScale = paused ? 0f : 1f;
        }
        
        OnPauseStateChanged?.Invoke(IsPaused, CurrentPauseType);
    }

    public void ResumeGame()
    {
        AudioManager.Instance.UnduckMusic();
        SetPaused(false);
    }

    public void StartCountdown()
    {
        IsInCountdown = true;
        Time.timeScale = 0f;
    }

    public void EndCountdown()
    {
        IsInCountdown = false;
        Time.timeScale = 1f;
    }

    public void PauseGame() => SetPaused(true, PauseType.EscPause);
    public void PauseForLevelUp() => SetPaused(true, PauseType.LevelUpPause);
}