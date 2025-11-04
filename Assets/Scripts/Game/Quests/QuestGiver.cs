using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestGiver : MonoBehaviour
{
    [SerializeField] QuestData quest;
    [NonSerialized] public bool _isReady = false;
    [NonSerialized] public bool _canBeInteractedWith = true;
    [NonSerialized] public bool _questComplete = false;
    private Renderer _renderer;
    

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
    }

    public void MakeReady()
    {
        _isReady = true;
        _renderer.material.color += new Color(0.1f, 0.04f, 0);
    }

    public void MakeNonReady()
    {
        if (_isReady)
        {
            _renderer.material.color -= new Color(0.1f, 0.04f, 0);
            _isReady = false;
        }
    }

    public void Interact()
    {
        if (!_questComplete)// If quest not complete, start it
        {
            quest._questGiver = this;
            quest.OnQuestStart();
            MakeNonReady();
            _canBeInteractedWith = false;
        }
        else // if it's complete
        {
            Debug.Log("Give Revard");
            MakeNonReady();
            _canBeInteractedWith = false;
        } 
    }

    public void SetQuestComplete()
    {
        _canBeInteractedWith = true;
        _questComplete = true;
    }

}
