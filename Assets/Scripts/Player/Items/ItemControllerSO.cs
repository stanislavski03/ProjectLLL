using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;

[CreateAssetMenu(fileName = "New Item Controller", menuName = "Items/Item Controller")]
public class ItemControllerSO : ScriptableObject
{
    private static ItemControllerSO _instance;
    public static ItemControllerSO Instance
    {
        get
        {
            if (_instance == null)
            {
                // Ищем существующий экземпляр
                var guids = AssetDatabase.FindAssets("t:ItemControllerSO");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<ItemControllerSO>(path);
                }

                // Если не нашли, создаем новый
                if (_instance == null)
                {
                    Debug.LogWarning("ItemControllerSO not found in project. Please create one via Assets/Create/Items/Item Controller");
                }
            }
            return _instance;
        }
    }




    [Header("Clear Pools")]
    [SerializeField] private List<ItemDataSO> AllItemsStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemAllTypesStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemUniversalStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemMagicStartPool = new List<ItemDataSO>();
    [SerializeField] private List<ItemDataSO> itemTecnoStartPool = new List<ItemDataSO>();

    [NonSerialized] public ItemType questType;



    [Header("Game Pools")]
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
                    AllItemsPool,
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

    // Для работы в Runtime (без AssetDatabase)
    public static void SetInstance(ItemControllerSO instance)
    {
        _instance = instance;
    }
    private void Awake()
    {
        ClearAllPools();
    }

    private void OnEnable()
    {
        // Автоматически устанавливаем себя как инстанс при загрузке
        if (_instance == null)
        {
            _instance = this;
        }
        else if (_instance != this)
        {
            Debug.LogWarning($"Multiple ItemControllerSO instances detected. Using existing instance: {_instance.name}", this);
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
             pool.Clear();
        }
        AllItemsPool.AddRange(AllItemsStartPool);
        itemAllTypesPool.AddRange(itemAllTypesStartPool);
        itemUniversalPool.AddRange(itemUniversalStartPool);
        itemMagicPool.AddRange(itemMagicStartPool);
        itemTecnoPool.AddRange(itemTecnoStartPool);


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
}