using UnityEngine;
using TMPro;

public class RaceStats : MonoBehaviour
{
    [Header("Настройки таймера")]
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private bool autoStart = true;

    [Header("Настройки счётчиков")]
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text killsText;
    
    private float currentTime = 0f;
    private bool isRunning = false;
    private bool isFinished = false;

    public static RaceStats Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        UpdateTimerDisplay(0f);
        
        if (autoStart)
        {
            StartTimer();
        }
    }

    void Update()
    {
        if (isRunning && !isFinished)
        {
            currentTime += Time.deltaTime;
            UpdateTimerDisplay(currentTime);
        }
    }

    private void OnEnable()
    {
        PlayerStatsSO.Instance._moneyChanged += UpdateCoinsDisplay;
        PlayerStatsSO.Instance._killsChanged += UpdateKillsDisplay;
    }

    private void OnDisable()
    {
        PlayerStatsSO.Instance._moneyChanged -= UpdateCoinsDisplay;
        PlayerStatsSO.Instance._killsChanged -= UpdateKillsDisplay;
    }

    private void UpdateTimerDisplay(float timeInSeconds)
    {
        int minutes = Mathf.FloorToInt(timeInSeconds / 60f);
        int seconds = Mathf.FloorToInt(timeInSeconds % 60f);
        
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StartTimer()
    {
        if (!isFinished)
        {
            isRunning = true;
        }
    }

    public void StopTimer()
    {
        if (isRunning)
        {
            isRunning = false;
        }
    }

    public void FinishRace()
    {
        if (!isFinished)
        {
            isRunning = false;
            isFinished = true;

        }
    }

    public void ResetTimer()
    {
        currentTime = 0f;
        isFinished = false;
        isRunning = false;
        UpdateTimerDisplay(0f);
    }

    public float GetCurrentTime()
    {
        return currentTime;
    }

    public string GetFormattedTime()
    {
        return timerText.text;
    }

    public bool IsTimerRunning()
    {
        return isRunning;
    }

    public bool IsRaceFinished()
    {
        return isFinished;
    }

    public void UpdateCoinsDisplay(float coins)
    {
        coinsText.text = coins.ToString();
    }

    public void UpdateKillsDisplay(float kills)
    {
        killsText.text = kills.ToString();
    }
}