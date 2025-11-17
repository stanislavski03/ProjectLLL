using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    public static GameMenuController Instance { get; private set; }
    [SerializeField] private GameObject _itemRewardMenu;

    [SerializeField] public GameObject _mutationRewardMenu;
    [SerializeField] public GameObject _mutationChestInfoPanel;
    [SerializeField] public GameObject _mutationChestCostPanel;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void GivePlayerItemReward()
    {
        _itemRewardMenu.SetActive(true);
    }

    public void GiveMutationReward() 
    {
        _mutationRewardMenu.SetActive(true);
    }
    
    public void RemoveFromSceneCostOnMutatiOnChest()
    {
        
        _mutationChestCostPanel.SetActive(false);
    }
    public void ShowCostOnMutatiOnChest( Transform _chestTransform)
    {
        _mutationChestCostPanel.SetActive(true);
        _mutationChestCostPanel.transform.position = new Vector3(_chestTransform.position.x, _chestTransform.position.y + 3, _chestTransform.position.z);
    }
    public void RemoveFromSceneWarningOnMutatiOnChest()
    {

        _mutationChestInfoPanel.SetActive(false);
    }
    public void ShowWarningOnMutatiOnChest(Transform _chestTransform)
    {
        _mutationChestInfoPanel.SetActive(true);
        _mutationChestInfoPanel.transform.position = new Vector3(_chestTransform.position.x, _chestTransform.position.y + 3, _chestTransform.position.z);
    }

}
