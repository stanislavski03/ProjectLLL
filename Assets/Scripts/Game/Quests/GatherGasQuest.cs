using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Gather Gas Quest", menuName = "Quests/New Gather Gas Quest")]
public class GatherGasQuest : QuestData
{
    [Header("Gather Gas Quest Settings")]
    public GameObject _gas;

    public override void OnQuestStart()
    {
        base.OnQuestStart();
        Generation.Instance.SpawnGasTanks(goal);
        QuestManager.Instance?.RegisterQuest(this);
        EnemySpawnManager.Instance.GainPoints(25);
    }

    public override void OnQuestFinish()
    {
        base.OnQuestFinish();
        QuestManager.Instance?.UnregisterQuest(this);

        EnemySpawnManager.Instance.GainDifficulty(0.2f);
    }
    
    public override void OnQuestCancel()
    {
        base.OnQuestCancel();
        QuestManager.Instance?.CancelQuest(this);
        
    }
}