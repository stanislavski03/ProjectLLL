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

    public AudioClip QuestClip;

    // События
    public event Action _onQuestRegistered;
    public event Action _onQuestFinished;
    public event Action _onQuestProgressUpdated;
    public event Action _onQuestCanceled;
    public event Action _onQuestTurnedIn;

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
            activeQuests.Add(quest);

            // Вызываем событие
            _onQuestRegistered?.Invoke();

            // Отправляем уведомление о принятии квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestAccepted(quest.QuestName);
            }
        }
    }

    public void UnregisterQuest(QuestData quest)
    {
        if (activeQuests.Contains(quest))
        {
            activeQuests.Remove(quest);
            completedQuests.Add(quest);

            // Вызываем событие
            _onQuestFinished?.Invoke();

            // Отправляем уведомление о завершении квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestCompleted(quest.QuestName);
            }
        }
    }

    public void TurnInQuest(QuestData quest)
    {
        if (completedQuests.Contains(quest))
        {
            completedQuests.Remove(quest);
            _onQuestTurnedIn?.Invoke();

            // Отправляем уведомление о сдаче квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestTurnedIn(quest.QuestName);
            }
        }
    }

    public void CancelQuest(QuestData quest)
    {
        if (!canceledQuests.Contains(quest) && (activeQuests.Contains(quest) || completedQuests.Contains(quest)))
        {
            canceledQuests.Add(quest);
            if (activeQuests.Contains(quest))
                activeQuests.Remove(quest);
            if (completedQuests.Contains(quest))
                completedQuests.Remove(quest);
            _onQuestCanceled?.Invoke();

            // Отправляем уведомление о провале квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestFailed(quest.QuestName);
            }
        }
    }

    public void OnQuestProgressUpdated(QuestData quest)
    {
        _onQuestProgressUpdated?.Invoke();

        // Отправляем уведомление о прогрессе (например, каждые 25%)
        if (QuestNotification.Instance != null && quest.progress > 0 && quest.progress < 100)
        {
            // Отправляем уведомление только при достижении определенных процентов
            if (quest.progress == 25 || quest.progress == 50 || quest.progress == 75 || quest.progress == 100)
            {
                QuestNotification.Instance.ShowQuestProgress(quest.QuestName, quest.progress);
            }
        }
    }

    public void OnEnemyKilled(int enemyType = 0)
    {
        var questsToUpdate = activeQuests.ToList();

        foreach (var quest in questsToUpdate)
        {
            if (!activeQuests.Contains(quest)) continue;

            if (quest is EnemyKillerQuest enemyKillerQuest && quest.active && !quest.finished)
            {
                if (enemyKillerQuest.TypesOfEnemy.Contains(enemyType))
                {
                    quest.UpdateQuest(1);

                    // Обновляем прогресс после изменения
                    OnQuestProgressUpdated(quest);
                }
            }
        }
    }

    public void GasTankGathered()
    {
        var questsToUpdate = activeQuests.ToList();
        foreach (var quest in questsToUpdate)
        {
            if (!activeQuests.Contains(quest)) continue;

            if (quest is GatherGasQuest gatherGasQuest && quest.active && !quest.finished)
            {
                quest.UpdateQuest(1);
                OnQuestProgressUpdated(quest);
            }
        }
    }

    public void BlizzardShieldProgressForOne()
    {
        var questsToUpdate = activeQuests.ToList();
        QuestData questNeeded = null;

        foreach (var quest in questsToUpdate)
        {
            if (!activeQuests.Contains(quest)) continue;

            if (quest is MagicBlizzardQuest magicBlizzardQuest && quest.active && !quest.finished)
            {
                questNeeded = quest;
                break;
            }
        }

        if (questNeeded != null && questNeeded.active && !questNeeded.finished)
        {
            questNeeded.UpdateQuest(1);
            OnQuestProgressUpdated(questNeeded);
        }
    }
}