using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Quest Data", menuName = "Quests/Quest Data")]
public class QuestData : QuestSO
{
    [Header("Quest Data Settings")]
    public int progress = 0;
    public bool active = false;
    public bool selected = false;
    public bool highLighted = false;
    public bool finished = false;
    public bool canceled = false;
    public bool turnedIn = false; // Новый флаг

    public string questInfo = "Выполни для меня задание а я тебе заплачу";
    public string questRewardInfo = "Отличная работа, возьми награду";

    public QuestGiver _questGiver;

    private int currentGoalCount = 0;

    private void OnEnable()
    {
        _onQuestUpdated += UpdateProgress;
        _onQuestStarted += OnQuestStart;
        _onQuestFinished += OnQuestFinish;
    }

    private void OnDisable()
    {
        _onQuestUpdated -= UpdateProgress;
        _onQuestStarted -= OnQuestStart;
        _onQuestFinished -= OnQuestFinish;
    }

    public void UpdateProgress(int goalCount)
    {
        currentGoalCount += goalCount;
        progress = Math.Clamp(currentGoalCount * 100 / goal, 0, 100);
        if (progress >= 100 && !finished)
        {
            FinishQuest();
        }
    }

    public virtual void OnQuestStart()
    {
        active = true;
        currentGoalCount = 0;
        progress = 0;
        finished = false;
        turnedIn = false;
    }

    public virtual void OnQuestFinish()
    {
        active = false;
        finished = true;
        progress = 100;
        _questGiver?.SetComplete();
    }

    public virtual void OnQuestTurnedIn()
    {
        turnedIn = true;
        QuestManager.Instance?.TurnInQuest(this);
    }

    public virtual void OnQuestCancel()
    {
        canceled = true;
        QuestManager.Instance?.CancelQuest(this);
    }

    public void OnSelect()
    {
        selected = !selected;
    }

    public void OnHighlighted()
    {
        highLighted = !highLighted;
    }
}