using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : EInteractable
{
    [SerializeField] MutationDataSO mutation;
    [SerializeField] private GameObject _rewardMenu;
    public long moneyRequired;

    public override void MakeReady()
    {
        _isReady = true;
        _renderer.material.color += new Color(0.5f, 0.54f, 0);
    }

    public override void MakeNonReady()
    {
        if (_isReady)
        {
            _renderer.material.color -= new Color(0.5f, 0.54f, 0);
            _isReady = false;
        }
    }

    public override void Interact()
    {
        if (PlayerStatsSO.Instance.Money >= moneyRequired)
        {
            PlayerStatsSO.Instance.Money -= moneyRequired;
            mutation.chest = this;
            _rewardMenu.SetActive(true);
            Destroy(gameObject);
        }
    }

}
