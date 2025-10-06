using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Data", menuName = "Weapons/Weapon Data")]
public class WeaponDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string weaponName;
    public string description;
    public Sprite icon;
    
    [Header("Base Stats")]
    public float baseDamage = 10f;
    public float baseCooldown = 1f;
    public int maxLevel = 6;
    
    [Header("Level Progression")]
    public LevelUpgrade[] levelUpgrades;
}

[System.Serializable]
public class LevelUpgrade
{
    public int level;
    public float areaBonus;
    public float damageBonus;
    public float cooldownReduction;
    public float speedBonus;
    public string upgradeDescription;
    
}