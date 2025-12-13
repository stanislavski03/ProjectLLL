using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class Chest : EInteractable
{
    [SerializeField] MutationDataSO mutation;

    private Animator openChestAnimator;


    private void Start()
    {
        openChestAnimator = GetComponent<Animator>();
        openChestAnimator.SetBool("IsOpen", false);
    }

    public override void MakeReady()
    {
        _isReady = true;
        _renderer.material.color += new Color(0.5f, 0.54f, 0);;
        GameMenuController.Instance.ShowCostOnMutatiOnChest(transform, MutationControllerSO.Instance._startChestCost.ToString());
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
        if (PlayerStatsSO.Instance.Money >= MutationControllerSO.Instance._startChestCost)
        {
            _canBeInteractedWith = false;
            openChestAnimator.SetBool("IsOpen", true);
            AudioManager.Instance.PlayOpenChest(MutationControllerSO.Instance.SuccessfulOpenChest);
            PlayerStatsSO.Instance.ChangeMoney(-MutationControllerSO.Instance._startChestCost);
            mutation.chest = this;
            GameMenuController.Instance.GiveMutationReward();
            GameMenuController.Instance.RemoveFromSceneCostOnMutatiOnChest();
            GameMenuController.Instance.RemoveFromSceneWarningOnMutatiOnChest();
            await UniTask.WaitForSeconds(1.5f);
            MutationControllerSO.Instance.ExpandCost();
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
                GameMenuController.Instance.ShowCostOnMutatiOnChest(transform, MutationControllerSO.Instance._startChestCost.ToString());


        }

    }

}
