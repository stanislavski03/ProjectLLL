using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : EInteractable
{
    [SerializeField] private QuestData _quest;
    private bool _questComplete = false;
    public ItemType _questType;

    public void SetQuest(QuestData Quest)
    {
        _quest = Quest;
    }

    public void SetQuestType(ItemType type)
    {
        _questType = type;
    }

    public override void MakeReady()
    {
        base.MakeReady();
        if (_questComplete)
        {
            GameMenuController.Instance.ShowQuestRewardInfo(transform, _quest.questRewardInfo);
        }
        else
        {
            GameMenuController.Instance.ShowQuestInfo(transform, _quest.questInfo);
        }
            
    }
    public override void MakeNonReady()
    {
        base.MakeNonReady();
        GameMenuController.Instance.RemoveFromSceneQuestInfo();
        GameMenuController.Instance.RemoveFromSceneQuestRewardInfo();
    }

    public bool QuestComplete { get { return _questComplete; } }
    public override void Interact()
    {
        if (!QuestComplete)
        {
            _quest._questGiver = this;
            _quest.OnQuestStart();
            MakeNonReady();
            _canBeInteractedWith = false;
        }
        else
        {
            ItemControllerSO.Instance.questType = _questType;
            GameMenuController.Instance.GivePlayerItemReward();
            MakeNonReady();
            _canBeInteractedWith = false;
        }
    }
    public override void SetComplete()
    {
        _canBeInteractedWith = true;
        _questComplete = true;
    }

}
