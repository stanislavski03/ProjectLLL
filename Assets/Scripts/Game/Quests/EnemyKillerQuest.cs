using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Killer Quest", menuName = "Quests/New Enemy Killer Quest")]
public class EnemyKillerQuest : QuestData
{
    [Header("Enemy Killer Quest Settings")]
    public List<int> TypesOfEnemy = new List<int>();

    public override void OnQuestStart()
    {
        base.OnQuestStart();
        QuestManager.Instance?.RegisterQuest(this);
    }

    public override void OnQuestFinish()
    {
        base.OnQuestFinish();
        QuestManager.Instance?.UnregisterQuest(this);

    }
    
    public override void OnQuestCancel()
    {
        base.OnQuestCancel();
        QuestManager.Instance?.CancelQuest(this);
        
    }
}