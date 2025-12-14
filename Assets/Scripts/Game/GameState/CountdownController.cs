using System.Collections;
using TMPro;
using UnityEngine;

public class CountdownController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countdownText;
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private float countdownDuration = 3f;

    private Coroutine countdownCoroutine;

    public void StartCountdown()
    {
        if (countdownCoroutine != null)
            StopCoroutine(countdownCoroutine);
            
        countdownCoroutine = StartCoroutine(CountdownRoutine());
    }

    private IEnumerator CountdownRoutine()
    {
        GameStateManager.Instance.StartCountdown();
        
        if (countdownPanel == null || countdownText == null)
        {
            EndCountdown();
            yield break;
        }

        countdownPanel.SetActive(true);

        float timer = countdownDuration;
        
        while (timer > 0)
        {
           timer -= Time.unscaledDeltaTime;
           int seconds = Mathf.CeilToInt(timer);
           countdownText.text = seconds.ToString();
           yield return null;
        }

        countdownText.text = "GO!";
        yield return new WaitForSecondsRealtime(0.5f);

        countdownPanel.SetActive(false);
        
        EndCountdown();
    }

    private void EndCountdown()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
            countdownCoroutine = null;
        }
        
        GameStateManager.Instance.EndCountdown();
    }
}