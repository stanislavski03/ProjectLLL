using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : EInteractable
{
    [SerializeField] QuestData quest;
    [NonSerialized] public bool _questComplete = false;
    [SerializeField] private GameObject _rewardMenu;
    [SerializeField] private ItemType _questType;

    public override void Interact()
    {
        if (!_questComplete)
        {
            quest._questGiver = this;
            quest.OnQuestStart();
            MakeNonReady();
            _canBeInteractedWith = false;
        }
        else 
        {
            ItemControllerSO.Instance.questType = _questType;
            _rewardMenu.SetActive(true);
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
