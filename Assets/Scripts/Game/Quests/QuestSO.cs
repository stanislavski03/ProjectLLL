using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New QuestSO", menuName = "Quests/Quest SO")]
public class QuestSO : SingletonScriptableObject<QuestSO>
{
    [Header("Basic Quest Settings")]
    public int goal = 1;
    public string QuestName;
    public string QuestDescription;
    public int GoldReward = 0;
    public List<ItemDataSO> ItemsReward = new List<ItemDataSO>();
    public int QuestId;

    public bool NeedReStartEvent = false;
    public bool HasStartAnimation = true;
    public bool HasFinishAnimation = true;

    public Action _onQuestStarted;
    public Action<int> _onQuestUpdated;
    public Action _onQuestFinished;

    public void StartQuest()
    {
        _onQuestStarted?.Invoke();
        HasStartAnimation = false;
    }

    public void UpdateQuest(int goalCount)
    {
        _onQuestUpdated?.Invoke(goalCount);
    }

    public void FinishQuest()
    {
        _onQuestFinished?.Invoke();
        HasFinishAnimation = false;
    }
}
