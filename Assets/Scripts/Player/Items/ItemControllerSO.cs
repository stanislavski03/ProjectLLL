using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;

[CreateAssetMenu(fileName = "New Item Controller", menuName = "Items/Item Controller")]
public class ItemControllerSO : SingletonScriptableObject<ItemControllerSO>
{




    [Header("Clear Pools")]
    [SerializeField] private List<ItemDataSO> AllItemsStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemUniversalStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemMagicStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemTecnoStartPool = new List<ItemDataSO>();

    [NonSerialized] public ItemType questType;



    [Header("Game Pools")]

    public List<ItemDataSO> ItemInventory = new List<ItemDataSO>();


    public List<ItemDataSO> AllItemsPool = new List<ItemDataSO>();

    public List<ItemDataSO> itemUniversalPool = new List<ItemDataSO>();
    public List<ItemDataSO> itemMagicPool = new List<ItemDataSO>();
    public List<ItemDataSO> itemTecnoPool = new List<ItemDataSO>();

    public List<ItemDataSO> itemActivePool = new List<ItemDataSO>();
    public List<ItemDataSO> itemPassivePool = new List<ItemDataSO>();

    public List<ItemDataSO> OnEnemyDeathPool = new List<ItemDataSO>();
    public List<ItemDataSO> OnSceneChangePool = new List<ItemDataSO>();
    public List<ItemDataSO> OnDamageGivePool = new List<ItemDataSO>();

    private List<List<ItemDataSO>> _allPools;
    public List<List<ItemDataSO>> AllPools
    {
        get
        {
            if (_allPools == null)
            {
                _allPools = new List<List<ItemDataSO>>
                {
                    ItemInventory,
                    AllItemsPool,
                    itemUniversalPool,
                    itemMagicPool,
                    itemTecnoPool,
                    itemActivePool,
                    itemPassivePool,
                    OnEnemyDeathPool,
                    OnSceneChangePool,
                    OnDamageGivePool
                };
            }
            return _allPools;
        }
    }

    public bool CheckPool(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Universal:
                return itemUniversalPool.Count != 0;
            case ItemType.Magic:
                return itemMagicPool.Count != 0;
            case ItemType.Tecno:
                return itemTecnoPool.Count != 0;
            default:
                return false; 
        }
    }


    private void Awake()
    {
        ClearAllPools();
    }

    public void InsertItemInPool(ItemDataSO item, List<ItemDataSO> Pool)
    {
        Pool.Add(item);
    }

    public void DeleteItemFromPool(ItemDataSO item, List<ItemDataSO> Pool)
    {
        Pool.Remove(item);
    }

    public void ClearAllPools()
    {
        foreach (List<ItemDataSO> pool in AllPools)
        {
             pool.Clear();
        }
        AllItemsPool.AddRange(AllItemsStartPool);
        itemUniversalPool.AddRange(itemUniversalStartPool);
        itemMagicPool.AddRange(itemMagicStartPool);
        itemTecnoPool.AddRange(itemTecnoStartPool);


    }

    public void DistributeItem(ItemDataSO item)
    {
        AllItemsPool.Remove(item);
        ItemInventory.Add(item);

        if (item.itemType == ItemType.Universal)
        {
            itemUniversalPool.Remove(item);
        }
        if (item.itemType == ItemType.Magic)
        {
            itemMagicPool.Remove(item);
        }
        if (item.itemType == ItemType.Tecno)
        {
            itemTecnoPool.Remove(item);
        }
        if (item.isActiveItem)
        {
            itemActivePool.Add(item);
        }
        if (!item.isActiveItem)
        {
            itemPassivePool.Add(item);
        }
        if (item.HasOnEnemyDeathEvent)
        {
            OnEnemyDeathPool.Add(item);
        }
        if (item.HasOnSceneChangeEvent)
        {
            OnSceneChangePool.Add(item);
        }
        if (item.HasOnDamageGiveEvent)
        {
            OnDamageGivePool.Add(item);
        }
    }

    public void ActivateOnEnemyDeathEvent(GameObject enemy)
    {
        if (OnEnemyDeathPool.Count > 0)
        {
            foreach (ItemDataSO item in OnEnemyDeathPool)
            {
                item.OnEnemyDeath(enemy);
            }
        }
    }

    public void ActivateOnSceneChangeEvent()
    {
        if (OnSceneChangePool.Count > 0)
        {
            foreach (ItemDataSO item in OnSceneChangePool)
            {
                item.OnSceneChange();
            }
        }
    }

    public void ActivateOnDamageGiveEvent()
    {
        if (OnDamageGivePool.Count > 0)
        {
            foreach (ItemDataSO item in OnDamageGivePool)
            {
                item.OnDamageGive();
            }
        }
    }
}