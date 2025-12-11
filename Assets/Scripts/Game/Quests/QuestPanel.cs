using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestPanel : MonoBehaviour
{
    public static QuestPanel Instance { get; private set; }

    public List<QuestData> AllActiveQuests;
    public List<QuestData> CompletedQuests;

    private int CurrentIndex;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
    }

    private void OnEnable()
    {
        QuestManager.Instance._onQuestRegistered += SetQuestsInfo;
        QuestManager.Instance._onQuestFinished += SetQuestsInfo;
    }

    private void OnDisable()
    {
        QuestManager.Instance._onQuestRegistered -= SetQuestsInfo;
        QuestManager.Instance._onQuestFinished -= SetQuestsInfo;
    }

    public void SetQuestsInfo()
    {
        ClearQuestList();

        if (QuestManager.Instance == null) return;
        if (QuestManager.Instance.activeQuests == null) return;

        AllActiveQuests = QuestManager.Instance.activeQuests;
        CompletedQuests = QuestManager.Instance.completedQuests;

        Transform[] quests = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) != null)
            {
                quests[i] = transform.GetChild(i);
            }
        }

        int currentUIElementIndex = 0;

        // Сначала отображаем активные квесты
        for (int i = 0; i < AllActiveQuests.Count; i++)
        {
            if (currentUIElementIndex >= quests.Length) break;
            if (AllActiveQuests[i] == null || quests[currentUIElementIndex] == null) continue;

            CurrentIndex = AllActiveQuests[i].QuestId;

            TextMeshProUGUI[] questsTexts = quests[currentUIElementIndex].GetComponentsInChildren<TextMeshProUGUI>();
            if (questsTexts != null && questsTexts.Length >= 2)
            {
                if (questsTexts[0] != null)
                    questsTexts[0].SetText(AllActiveQuests[i].QuestName);
                if (questsTexts[1] != null)
                    questsTexts[1].SetText(AllActiveQuests[i].QuestDescription);
            }
            
            UnityEngine.UI.Slider questSlider = quests[currentUIElementIndex].GetComponentInChildren<UnityEngine.UI.Slider>();
            if (questSlider != null)
            {
                questSlider.value = AllActiveQuests[i].progress;
            }
            
            UnityEngine.UI.Image panelImage = quests[currentUIElementIndex].GetComponent<UnityEngine.UI.Image>();
            if (panelImage != null)
            {
                panelImage.color = Color.black;
            }

            quests[currentUIElementIndex].gameObject.SetActive(true);
            currentUIElementIndex++;
        }

        // Затем отображаем завершенные квесты
        for (int i = 0; i < CompletedQuests.Count; i++)
        {
            if (currentUIElementIndex >= quests.Length) break;
            if (CompletedQuests[i] == null || quests[currentUIElementIndex] == null) continue;

            CurrentIndex = CompletedQuests[i].QuestId;

            TextMeshProUGUI[] questsTexts = quests[currentUIElementIndex].GetComponentsInChildren<TextMeshProUGUI>();
            if (questsTexts != null && questsTexts.Length >= 2)
            {
                if (questsTexts[0] != null)
                    questsTexts[0].SetText(CompletedQuests[i].QuestName);
                if (questsTexts[1] != null)
                    questsTexts[1].SetText(CompletedQuests[i].QuestDescription);
            }
            
            UnityEngine.UI.Slider questSlider = quests[currentUIElementIndex].GetComponentInChildren<UnityEngine.UI.Slider>();
            if (questSlider != null)
            {
                questSlider.value = 100;
            }
            
            UnityEngine.UI.Image panelImage = quests[currentUIElementIndex].GetComponent<UnityEngine.UI.Image>();
            if (panelImage != null)
            {
                panelImage.color = Color.green;
            }

            quests[currentUIElementIndex].gameObject.SetActive(true);
            currentUIElementIndex++;
        }
    }

    public void ClearQuestList()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}