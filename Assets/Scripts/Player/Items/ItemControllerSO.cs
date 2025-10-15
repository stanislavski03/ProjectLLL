using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName = "New Item Controller", menuName = "Items/Item Controller")]
public class ItemControllerSO : SingletonScriptableObject<ItemControllerSO>
{

    [Header("Item Pools")]
    public List<ItemDataSO> AllItemsPool = new List<ItemDataSO>();

    public List<ItemDataSO> itemAllTypesPool = new List<ItemDataSO>();
    public List<ItemDataSO> itemUniversalPool = new List<ItemDataSO>();
    public List<ItemDataSO> itemMagicPool = new List<ItemDataSO>();
    public List<ItemDataSO> itemTecnoPool = new List<ItemDataSO>();

    public List<ItemDataSO> itemActivePool = new List<ItemDataSO>();
    public List<ItemDataSO> itemPassivePool = new List<ItemDataSO>();

    public List<ItemDataSO> OnEnemyDeathPool = new List<ItemDataSO>();
    public List<ItemDataSO> OnSceneChangePool = new List<ItemDataSO>();

    private List<List<ItemDataSO>> _allPools;
    public List<List<ItemDataSO>> AllPools
    {
        get
        {
            if (_allPools == null)
            {
                _allPools = new List<List<ItemDataSO>>
                {
                    itemAllTypesPool,
                    itemUniversalPool,
                    itemMagicPool,
                    itemTecnoPool,
                    itemActivePool,
                    itemPassivePool,
                    OnEnemyDeathPool,
                    OnSceneChangePool
                };
            }
            return _allPools;
        }
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
            for (int i = pool.Count - 1; i >= 0; i--)
            {
                ItemDataSO item = pool[i];
                pool.RemoveAt(i);

                if (!AllItemsPool.Contains(item))
                {
                    AllItemsPool.Add(item);
                }
            }
        }
    }

    public void DistributeItem(ItemDataSO item)
    {
        AllItemsPool.Remove(item);
        itemAllTypesPool.Add(item);

        if (item.itemType == ItemType.Universal)
        {
            itemUniversalPool.Add(item);
        }
        if (item.itemType == ItemType.Magic)
        {
            itemMagicPool.Add(item);
        }
        if (item.itemType == ItemType.Tecno)
        {
            itemTecnoPool.Add(item);
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
    }

    public void ActivateOnEnemyDeathEvent(Transform enemyTransform)
    {
        if (OnEnemyDeathPool.Count > 0)
        {
            foreach (ItemDataSO item in OnEnemyDeathPool)
            {
                item.OnEnemyDeath();
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



}