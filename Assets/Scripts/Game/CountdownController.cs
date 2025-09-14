using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private float countdownDuration = 3f;

    private Coroutine countdownCoroutine;
    private Action onCountdownComplete;

    private void OnEnable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged += OnStateChanged;
        }
    }

    private void OnDisable()
    {
        if (GameStateManager.Instance != null)
        {
            GameStateManager.Instance.OnStateChanged -= OnStateChanged;
        }
        
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
    }

    private void OnStateChanged(GameState state)
    {
        // Скрываем таймер всегда, кроме CountdownState
        if (!(state is CountdownState))
        {
            if (countdownCoroutine != null)
            {
                StopCoroutine(countdownCoroutine);
                countdownCoroutine = null;
            }
            countdownPanel.SetActive(false);
        }
    }

    public void StartCountdown(Action onComplete)
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }
        
        onCountdownComplete = onComplete;
        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        if (countdownPanel == null || countdownText == null)
        {
            onCountdownComplete?.Invoke();
            yield break;
        }

        countdownPanel.SetActive(true);

        float timer = countdownDuration;
        
        while (timer > 0f)
        {
            timer -= Time.unscaledDeltaTime; // Используем unscaled время
            int seconds = Mathf.CeilToInt(timer);
            countdownText.text = seconds.ToString();
            yield return null;
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f); // Realtime чтобы не зависело от Time.timeScale

        countdownPanel.SetActive(false);
        countdownCoroutine = null;
        
        onCountdownComplete?.Invoke();
    }
}