using UnityEngine;
using TMPro;

public class DeathOrWinPanel : MonoBehaviour
{
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text coinsText;
    [SerializeField] private TMP_Text killsText;

    void OnEnable()
    {
        timerText.text = RaceStats.Instance.timerText.text;
        coinsText.text = RaceStats.Instance.coinsText.text;
        killsText.text = RaceStats.Instance.killsText.text;
    }

}