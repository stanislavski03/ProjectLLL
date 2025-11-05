using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;
using System;

[CreateAssetMenu(fileName = "New Mutation Controller", menuName = "Mutations/Mutation Controller")]
public class MutationControllerSO : ScriptableObject
{
    [Header("Player Mutations Pool")]
    public List<MutationDataSO> AllMutationsPool = new List<MutationDataSO>();
    public List<MutationDataSO> PlayerMutationsPool = new List<MutationDataSO>();

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
        mutation.OnPick();

        PlayerMutationsPool.Add(mutation);
    }

    public void DeleteMutation(MutationDataSO mutation)
    {
        mutation.OnDelete();

        PlayerMutationsPool.Remove(mutation);
    }



}
