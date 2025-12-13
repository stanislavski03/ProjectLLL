using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : EInteractable
{
    [SerializeField] private QuestData _quest;
    private bool _questComplete = false;
    private bool _questTurnedIn = false;
    public ItemType _questType;
    public bool _transitionQuest = false;

    public Animator _questAnimator;

    [SerializeField] private Renderer _questGiverHatchRenderer;

    public Material _techno;
    public Material _magic;
    public Material _universal;
    public Material _transition;


    public void SetQuest(QuestData Quest)
    {
        _quest = Quest;
    }

    public void SetQuestType(ItemType type)
    {
        _questType = type;
        if(type == ItemType.Tecno)
        {
            _questGiverHatchRenderer.material = _techno;
        }
        else if (type == ItemType.Magic)
        {
            _questGiverHatchRenderer.material = _magic;
        }
        else if (type == ItemType.Universal)
        {
            _questGiverHatchRenderer.material = _universal;
        }
    }
    public void SetQuestTransition()
    {
        _transitionQuest = true;
        _questGiverHatchRenderer.material = _transition;
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
    public bool QuestTurnedIn { get { return _questTurnedIn; } }
    
    public override void Interact()
    {
        AudioManager.Instance.PlayQuest(QuestManager.Instance.QuestClip);
        
        if (!QuestComplete)
        {
            // Если это переходный квест, показываем окно подтверждения
            if (_transitionQuest && TransitionQuestStartPanel.Instance != null)
            {
                TransitionQuestStartPanel.Instance.ShowTransitionQuestConfirmation(_quest, this);
            }
            else
            {
                // Обычный квест - начинаем сразу
                StartRegularQuest();
            }
        }
        else
        {
            // Логика сдачи квеста
            ItemControllerSO.Instance.questType = _questType;
            if (!_transitionQuest)
            {
                GameMenuController.Instance.GivePlayerItemReward();
            }
            else
            {
                TransitionManager.Instance.TransitPlayerToNextLevel();
            }

            // Отмечаем квест как сданный
            _questTurnedIn = true;
            if (_quest != null)
            {
                _quest.turnedIn = true;
                QuestManager.Instance?.TurnInQuest(_quest);
            }
            
            MakeNonReady();
            _canBeInteractedWith = false;
            _questAnimator.SetBool("IsActive", false);
            _questAnimator.SetBool("IsFinished", false);
        }
    }
    
    // Новый метод для начала обычного квеста
    private void StartRegularQuest()
    {
        _quest._questGiver = this;
        _quest.OnQuestStart();
        
        if (QuestManager.Instance != null)
        {
            QuestManager.Instance.RegisterQuest(_quest);
        }
        
        MakeNonReady();
        _canBeInteractedWith = false;
        
        if (_questAnimator != null)
        {
            _questAnimator.SetBool("IsActive", true);
        }
    }
    
    public override void SetComplete()
    {
        _canBeInteractedWith = true;
        _questComplete = true;
        
        if (_questAnimator != null)
        {
            _questAnimator.SetBool("IsFinished", true);
        }
    }
}