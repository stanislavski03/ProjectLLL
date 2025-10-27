using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    [SerializeField] public List<QuestData> completedQuests = new List<QuestData>();
    [SerializeField] public List<QuestData> activeQuests = new List<QuestData>();
    [SerializeField] private List<QuestData> canceledQuests = new List<QuestData>();

    public event Action _onQuestRegistered;
    public event Action _onQuestFinished;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterQuest(QuestData quest)
    {
        if (!activeQuests.Contains(quest))
        {
            _onQuestRegistered?.Invoke();
            activeQuests.Add(quest);
            Debug.Log($"Начат квест: {quest.QuestName}. Активных квестов: {activeQuests.Count}");
        }
    }

    public void UnregisterQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest))
        {
            _onQuestFinished?.Invoke();
            activeQuests.Remove(quest);
            completedQuests.Add(quest);
            Debug.Log($"Квест пройден: {quest.QuestName}. Активных квестов: {activeQuests.Count}");
        }
    }

    public void CancelQuest(QuestData quest)
    {
        if (!canceledQuests.Contains(quest) && activeQuests.Contains(quest))
        {
            canceledQuests.Add(quest);
            activeQuests.Remove(quest);
            Debug.Log($"Квест отменён: {quest.QuestName}. Активных квестов: {activeQuests.Count}");
        }
    }

    public void OnEnemyKilled(int enemyType = 0)
    {
        Debug.Log($"Враг убит! Тип врага: {enemyType}");

        var questsToUpdate = activeQuests.ToList();

        foreach (var quest in questsToUpdate)
        {
            if (!activeQuests.Contains(quest)) continue;

            if (quest is EnemyKillerQuest enemyKillerQuest && quest.active && !quest.finished)
            {
                if (enemyKillerQuest.TypesOfEnemy.Contains(enemyType))
                {
                    Debug.Log($"Обновился квест: {quest.QuestName} (тип врага: {enemyType})");
                    quest.UpdateQuest(1);
                }
            }
        }
    }
}