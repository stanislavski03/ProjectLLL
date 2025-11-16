using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuController : MonoBehaviour
{
    public static GameMenuController Instance { get; private set; }
    [SerializeField] private GameObject _itemRewardMenu;
    [SerializeField] private GameObject _mutationRewardMenu;

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


    
}
