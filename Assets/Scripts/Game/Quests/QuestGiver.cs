using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : EInteractable
{
    [SerializeField] private QuestData _quest;
    private bool _questComplete = false;
    public ItemType _questType;
    public bool _transitionQuest = false;

    public Animator _questAnimator;

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
        _questAnimator.SetBool("IsPlayerNear", true);
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
        _questAnimator.SetBool("IsPlayerNear", false);
        GameMenuController.Instance.RemoveFromSceneQuestInfo();
        GameMenuController.Instance.RemoveFromSceneQuestRewardInfo();
    }

    public bool QuestComplete { get { return _questComplete; } }
    public override void Interact()
    {
        AudioManager.Instance.PlayQuest(QuestManager.Instance.QuestClip);
        if (!QuestComplete)
        {
            _quest._questGiver = this;
            _quest.OnQuestStart();
            MakeNonReady();
            _canBeInteractedWith = false;
            _questAnimator.SetBool("IsActive", true);
        }
        else
        {
            ItemControllerSO.Instance.questType = _questType;
            if (!_transitionQuest)
            {
                GameMenuController.Instance.GivePlayerItemReward();
            }
            else
            {
                TransitionManager.Instance.TransitPlayerToNextLevel();
            }


            MakeNonReady();
            _canBeInteractedWith = false;
            _questAnimator.SetBool("IsActive", false);
            
        }
    }
    public override void SetComplete()
    {
        _canBeInteractedWith = true;
        _questComplete = true;
        _questAnimator.SetBool("IsFinished", true);
    }

}
