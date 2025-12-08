using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Chest : EInteractable
{
    [SerializeField] MutationDataSO mutation;

    public long moneyRequired;

    public override void MakeReady()
    {
        _isReady = true;
        _renderer.material.color += new Color(0.5f, 0.54f, 0);
        //TextMeshProUGUI ChestCost = _cost.GetComponentInChildren<TextMeshProUGUI>();
        //ChestCost.text = moneyRequired.ToString();
        GameMenuController.Instance.ShowCostOnMutatiOnChest(transform);
    }
    public override void MakeNonReady()
    {
        if (_isReady)
        {
            _renderer.material.color -= new Color(0.5f, 0.54f, 0);
            _isReady = false;
            GameMenuController.Instance.RemoveFromSceneCostOnMutatiOnChest();
        }
    }

    public async override void Interact()
    {
        if (PlayerStatsSO.Instance.Money >= moneyRequired)
        {
            AudioManager.Instance.PlayOpenChest(MutationControllerSO.Instance.SuccessfulOpenChest);
            PlayerStatsSO.Instance.Money -= moneyRequired;
            mutation.chest = this;
            GameMenuController.Instance.GiveMutationReward();
            GameMenuController.Instance.RemoveFromSceneCostOnMutatiOnChest();
            GameMenuController.Instance.RemoveFromSceneWarningOnMutatiOnChest();
            Destroy(gameObject);
        }
        else
        {
            AudioManager.Instance.PlayOpenChest(MutationControllerSO.Instance.ErrorOpenChest);
            GameMenuController.Instance.RemoveFromSceneCostOnMutatiOnChest();

            GameMenuController.Instance.ShowWarningOnMutatiOnChest(transform);
            await UniTask.WaitForSeconds(1);
            GameMenuController.Instance.RemoveFromSceneWarningOnMutatiOnChest();
            if (_isReady)
                GameMenuController.Instance.ShowCostOnMutatiOnChest(transform);


        }

    }

}
