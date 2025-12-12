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

    // Старые события для совместимости
    public event Action _onQuestRegistered;
    public event Action _onQuestFinished;
    
    // Новые события с параметрами
    public event Action<QuestData> _onQuestRegisteredDetailed;
    public event Action<QuestData> _onQuestFinishedDetailed;
    public event Action<QuestData> _onQuestProgressUpdated;
    public event Action<QuestData> _onQuestCanceled;
    public event Action<QuestData> _onQuestTurnedIn;

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
            
            // Вызываем оба события для совместимости
            _onQuestRegistered?.Invoke();
            _onQuestRegisteredDetailed?.Invoke(quest);
            
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
            
            // Вызываем оба события для совместимости
            _onQuestFinished?.Invoke();
            _onQuestFinishedDetailed?.Invoke(quest);
            
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
            _onQuestTurnedIn?.Invoke(quest);
            
            // Отправляем уведомление о сдаче квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestTurnedIn(quest.QuestName);
            }
        }
    }

    public void CancelQuest(QuestData quest)
    {
        if (!canceledQuests.Contains(quest) && activeQuests.Contains(quest))
        {
            canceledQuests.Add(quest);
            activeQuests.Remove(quest);
            _onQuestCanceled?.Invoke(quest);
            
            // Отправляем уведомление о провале квеста
            if (QuestNotification.Instance != null)
            {
                QuestNotification.Instance.ShowQuestFailed(quest.QuestName);
            }
        }
    }

    public void OnQuestProgressUpdated(QuestData quest)
    {
        _onQuestProgressUpdated?.Invoke(quest);
        
        // Отправляем уведомление о прогрессе (например, каждые 25%)
        if (QuestNotification.Instance != null && quest.progress % 25 == 0 && quest.progress > 0)
        {
            QuestNotification.Instance.ShowQuestProgress(quest.QuestName, quest.progress);
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
            }
        }
    }
    public void BlizzardShieldProgressForOne()
    {
        var questsToUpdate = activeQuests.ToList();
        QuestData questNeeded = null;
        if (questNeeded == null)
        {
            foreach (var quest in questsToUpdate)
            {
                if (!activeQuests.Contains(quest)) continue;

                if (quest is MagicBlizzardQuest magicBlizzardQuest && quest.active && !quest.finished)
                {
                    questNeeded = quest;
                }
            }
        }

        if (questNeeded != null && questNeeded.active && !questNeeded.finished)
        {
            questNeeded.UpdateQuest(1);
        }
    }

}