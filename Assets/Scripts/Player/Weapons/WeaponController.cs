using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "New Weapon Controller", menuName = "Weapons/Weapon Controller")]
public class WeaponController : ScriptableObject
{

    private static WeaponController _instance;
    public static WeaponController Instance
    {
        get
        {
            if (_instance == null)
            {
                // ���� ������������ ���������
                var guids = AssetDatabase.FindAssets("t:WeaponController");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<WeaponController>(path);
                }

                // ���� �� �����, ������� �����
                if (_instance == null)
                {
                    Debug.LogWarning("WeaponController not found in project. Please create one via Assets/Create/Items/Item Controller");
                }
            }
            return _instance;
        }
    }

    public List<WeaponDataSO> currentPlayerWeapons;

    public void ClearPool()
    {
        currentPlayerWeapons.RemoveRange(0, currentPlayerWeapons.Count);
    }
}