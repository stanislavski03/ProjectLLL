using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownText;
    [SerializeField] private GameObject _countdownPanel;
    [SerializeField] private float _countdownDuration = 3f;

    public static event Action OnCountdownStarted;  // Новое событие - начало отсчета
    public static event Action OnCountdownFinished; // Отсчет завершен
    public static bool IsCountdownActive { get; private set; }

    private void Awake()
    {
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        _countdownPanel.SetActive(false);
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Gameplay)
        {
            StartCoroutine(StartCountdown());
        }
        else if (newGameState == GameState.Paused)
        {
            // При паузе сразу останавливаем отсчет
            StopAllCoroutines();
            _countdownPanel.SetActive(false);
            IsCountdownActive = false;
            OnCountdownFinished?.Invoke(); // Уведомляем что отсчет прерван
        }
    }

    private IEnumerator StartCountdown()
    {
        IsCountdownActive = true;
        _countdownPanel.SetActive(true);
        OnCountdownStarted?.Invoke(); // Уведомляем о НАЧАЛЕ отсчета

        // Отсчет 3, 2, 1...
        for (float time = _countdownDuration; time > 0; time--)
        {
            _countdownText.text = Mathf.CeilToInt(time).ToString();
            yield return new WaitForSeconds(1f);
        }

        _countdownText.text = "GO!";
        yield return new WaitForSeconds(0.5f);
        
        // Завершение отсчета
        _countdownPanel.SetActive(false);
        IsCountdownActive = false;
        OnCountdownFinished?.Invoke(); // Уведомляем о ЗАВЕРШЕНИИ отсчета
    }

    public void SkipCountdown()
    {
        StopAllCoroutines();
        _countdownPanel.SetActive(false);
        IsCountdownActive = false;
        OnCountdownFinished?.Invoke();
    }
}