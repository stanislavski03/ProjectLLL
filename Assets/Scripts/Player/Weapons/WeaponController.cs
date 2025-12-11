using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[CreateAssetMenu(fileName = "New Weapon Controller", menuName = "Weapons/Weapon Controller")]
public class WeaponController : SingletonScriptableObject<WeaponController>
{

    public List<WeaponDataSO> currentPlayerWeapons;

    public void ClearPool()
    {
        currentPlayerWeapons.RemoveRange(0, currentPlayerWeapons.Count);
    }
}