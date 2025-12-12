using System;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Blood Experiment Quest", menuName = "Quests/Blood Experiment Quest")]
public class BloodExperimentQuest : QuestData
{
    public override async void OnQuestStart()
    {
        base.OnQuestStart();
        PlayerHP.Instance.DamageInPercent(goal);
        QuestManager.Instance?.RegisterQuest(this);
        await UniTask.WaitForSeconds(3);
        UpdateQuest(goal);
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