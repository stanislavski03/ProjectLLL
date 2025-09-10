using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private float _countdownDuration = 3f;

    public static event Action OnCountdownStarted;
    public static event Action OnCountdownFinished;
    public static bool IsCountdownActive { get; private set; }

    private GameState _previousState; // Запоминаем предыдущее состояние

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        _countdownPanel.SetActive(false);
        _previousState = GameStateManager.Instance.CurrentGameState;
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        // Запоминаем предыдущее состояние перед изменением
        GameState oldState = _previousState;
        _previousState = newGameState;

        if (newGameState == GameState.Gameplay)
        {
            // Запускаем отсчет ТОЛЬКО если предыдущее состояние было Paused
            if (oldState == GameState.Paused)
            {
                StartCoroutine(StartCountdown());
            }
            else
            {
                // Если переходим не из Paused (например из LevelUpPaused), сразу уведомляем о завершении
                OnCountdownFinished?.Invoke();
            }
        }
        else if (newGameState == GameState.Paused)
        {
            StopAllCoroutines();
            _countdownPanel.SetActive(false);
            IsCountdownActive = false;
            OnCountdownFinished?.Invoke();
        }
        // LevelUpPaused игнорируем - не делаем ничего
    }

    private IEnumerator StartCountdown()
    {
        IsCountdownActive = true;
        _countdownPanel.SetActive(true);
        OnCountdownStarted?.Invoke();

        for (float time = _countdownDuration; time > 0; time--)
        {
            _countdownText.text = Mathf.CeilToInt(time).ToString();
            yield return new WaitForSeconds(1f);
        }

        _countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        
        _countdownPanel.SetActive(false);
        IsCountdownActive = false;
        OnCountdownFinished?.Invoke();
    }

    public void SkipCountdown()
    {
        StopAllCoroutines();
        _countdownPanel.SetActive(false);
        IsCountdownActive = false;
        OnCountdownFinished?.Invoke();
    }
}