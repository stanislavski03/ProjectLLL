using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public static GameMenuController Instance { get; private set; }
    
    [SerializeField] public GameObject _mutationRewardMenu;
    [SerializeField] public GameObject _mutationChestInfoPanel;
    [SerializeField] public GameObject _mutationChestCostPanel;

    [SerializeField] public GameObject _itemRewardMenu;
    [SerializeField] public GameObject _questInfoPanel;
    [SerializeField] public GameObject _questRewardInfoPanel;


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

    public void RemoveFromSceneQuestInfo()
    {
        
        _questInfoPanel.SetActive(false);
    }
    public void ShowQuestInfo(Transform _chestTransform, string  Text)
    {
        _questInfoPanel.SetActive(true);
        _questInfoPanel.transform.position = new Vector3(_chestTransform.position.x, _chestTransform.position.y + 6, _chestTransform.position.z);
        _questInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = Text;
    }
    public void RemoveFromSceneQuestRewardInfo()
    {

        _questRewardInfoPanel.SetActive(false);
    }
    public void ShowQuestRewardInfo(Transform _chestTransform, string  Text)
    {
        _questRewardInfoPanel.SetActive(true);
        _questRewardInfoPanel.transform.position = new Vector3(_chestTransform.position.x, _chestTransform.position.y + 6, _chestTransform.position.z);
        _questRewardInfoPanel.GetComponentInChildren<TextMeshProUGUI>().text = Text;
    }

}
