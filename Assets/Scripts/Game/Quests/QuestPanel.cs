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
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance._onQuestRegistered += SetQuestsInfo;
            QuestManager.Instance._onQuestFinished += SetQuestsInfo;
            QuestManager.Instance._onQuestProgressUpdated += SetQuestsInfo;
            QuestManager.Instance._onQuestCanceled += SetQuestsInfo;
        }
    }

    private void OnDisable()
    {
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance._onQuestRegistered -= SetQuestsInfo;
            QuestManager.Instance._onQuestFinished -= SetQuestsInfo;
            QuestManager.Instance._onQuestProgressUpdated -= SetQuestsInfo;
            QuestManager.Instance._onQuestCanceled -= SetQuestsInfo;
        }
    }

    // Старый метод без параметра (оставляем для обратной совместимости)
    public void SetQuestsInfo()
    {
        UpdateQuestsDisplay();
    }

    // Новый метод с параметром
    public void SetQuestsInfo(QuestData quest)
    {
        UpdateQuestsDisplay();
        
        // Можно добавить дополнительную логику для конкретного квеста
        // Например, подсветить изменившийся квест
    }

    private void UpdateQuestsDisplay()
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

        // Отображаем только активные квесты и выполненные, но не сданные
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
                if (AllActiveQuests[i].finished)
                {
                    // Выполненный, но не сданный квест - желтый цвет
                    panelImage.color = Color.yellow;
                }
                else
                {
                    // Активный квест - черный цвет
                    panelImage.color = Color.black;
                }
            }

            quests[currentUIElementIndex].gameObject.SetActive(true);
            currentUIElementIndex++;
        }

        // Отображаем только выполненные, но не сданные квесты из списка completedQuests
        for (int i = 0; i < CompletedQuests.Count; i++)
        {
            // Пропускаем квесты, которые уже были отображены в списке активных
            bool alreadyDisplayed = false;
            foreach (var activeQuest in AllActiveQuests)
            {
                if (activeQuest.QuestId == CompletedQuests[i].QuestId)
                {
                    alreadyDisplayed = true;
                    break;
                }
            }
            
            if (alreadyDisplayed) continue;
            
            // Проверяем, сдан ли квест
            if (CompletedQuests[i]._questGiver != null && CompletedQuests[i]._questGiver.QuestTurnedIn ||
                CompletedQuests[i].turnedIn)
            {
                // Квест сдан - пропускаем его
                continue;
            }

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
                // Выполненный, но не сданный квест - желтый цвет
                panelImage.color = Color.yellow;
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