using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.Linq;

[CreateAssetMenu(fileName = "New Mutation Controller", menuName = "Mutations/Mutation Controller")]
public class MutationControllerSO : ScriptableObject
{
    [System.Serializable]
    public class MutationEntry
    {
        public MutationDataSO mutation;
        public int count;
        
        public MutationEntry(MutationDataSO mutation)
        {
            this.mutation = mutation;
            this.count = 1;
        }
    }

    [Header("All Available Mutations")]
    public List<MutationDataSO> AllMutationsPool = new List<MutationDataSO>();
    
    // Публичное свойство для обратной совместимости
    public List<MutationDataSO> PlayerMutationsPool
    {
        get
        {
            // Конвертируем список записей обратно в плоский список для совместимости
            var flatList = new List<MutationDataSO>();
            foreach (var entry in PlayerMutations)
            {
                for (int i = 0; i < entry.count; i++)
                {
                    flatList.Add(entry.mutation);
                }
            }
            return flatList;
        }
    }
    
    [Header("Player Mutations")]
    public List<MutationEntry> PlayerMutations = new List<MutationEntry>();


    public AudioClip SuccessfulOpenChest;
    public AudioClip ErrorOpenChest;

    private static MutationControllerSO _instance;
    public static MutationControllerSO Instance
    {
        get
        {
            if (_instance == null)
            {
                var guids = AssetDatabase.FindAssets("t:MutationControllerSO");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<MutationControllerSO>(path);
                }

                if (_instance == null)
                {
                    Debug.LogWarning("MutationControllerSO not found in project. Please create one via Assets/Create/Mutations/Mutation Controller");
                }
            }
            return _instance;
        }
    }

    public void AddMutation(MutationDataSO mutation)
    {
        // Проверяем, есть ли уже такая мутация
        var existingEntry = PlayerMutations.FirstOrDefault(m => m.mutation == mutation);
        
        if (existingEntry != null)
        {
            // Если мутация уже есть, увеличиваем счетчик
            existingEntry.count++;
            existingEntry.mutation.OnPick();
        }
        else
        {
            // Если мутации нет, добавляем новую запись
            mutation.OnPick();
            PlayerMutations.Add(new MutationEntry(mutation));
        }
    }

    public void DeleteMutation(MutationDataSO mutation)
    {
        var existingEntry = PlayerMutations.FirstOrDefault(m => m.mutation == mutation);
        
        if (existingEntry != null)
        {
            if (existingEntry.count > 1)
            {
                // Если есть несколько экземпляров, уменьшаем счетчик
                existingEntry.count--;
                existingEntry.mutation.OnDelete();
            }
            else
            {
                // Если только один экземпляр, удаляем запись полностью
                existingEntry.mutation.OnDelete();
                PlayerMutations.Remove(existingEntry);
            }
        }
    }

    public void ClearAllMutations()
    {
        foreach (var entry in PlayerMutations)
        {
            entry.mutation.OnDelete();
        }
        PlayerMutations.Clear();
    }

    public bool ContainsMutation(MutationDataSO mutation)
    {
        return PlayerMutations.Any(m => m.mutation == mutation);
    }

    // Старый метод для обратной совместимости
    public bool IsAlreadyContains(MutationDataSO mutation)
    {
        return ContainsMutation(mutation);
    }

    // Старый метод для обратной совместимости
    public int CalculateMutationCount(MutationDataSO mutation)
    {
        var entry = PlayerMutations.FirstOrDefault(m => m.mutation == mutation);
        return entry?.count ?? 0;
    }
    
    // Новый метод для получения всех уникальных мутаций
    public List<MutationEntry> GetUniquePlayerMutations()
    {
        return new List<MutationEntry>(PlayerMutations);
    }
    
    // Метод для получения списка мутаций как в старом формате
    public List<MutationDataSO> GetFlatMutationList()
    {
        return PlayerMutationsPool;
    }
}