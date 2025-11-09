using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Chest : EInteractable
{
    [SerializeField] MutationDataSO mutation;
    [SerializeField] private GameObject _rewardMenu;
    [SerializeField] private GameObject _cost;
    [SerializeField] private GameObject _warning;
    public long moneyRequired;

    public override void MakeReady()
    {
        _isReady = true;
        _renderer.material.color += new Color(0.5f, 0.54f, 0);
        TextMeshProUGUI ChestCost = _cost.GetComponentInChildren<TextMeshProUGUI>();
        ChestCost.text = moneyRequired.ToString();
        _cost.SetActive(true);
        _cost.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
    }

    public override void MakeNonReady()
    {
        if (_isReady)
        {
            _renderer.material.color -= new Color(0.5f, 0.54f, 0);
            _isReady = false;
            _cost.SetActive(false);
        }
    }

    public async override void Interact()
    {
        if (PlayerStatsSO.Instance.Money >= moneyRequired)
        {
            PlayerStatsSO.Instance.Money -= moneyRequired;
            mutation.chest = this;
            _rewardMenu.SetActive(true);
            _cost.SetActive(false);
            _warning.SetActive(false);
            Destroy(gameObject);
        }
        else
        {
            _cost.SetActive(false);
            _warning.SetActive(true);
            _warning.transform.position = new Vector3(transform.position.x, transform.position.y + 3, transform.position.z);
            await UniTask.WaitForSeconds(1);
            _warning.SetActive(false);
            if (_isReady)
                _cost.SetActive(true);
            

        }

    }

}
