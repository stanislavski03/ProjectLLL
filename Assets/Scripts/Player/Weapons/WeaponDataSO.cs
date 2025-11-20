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
    public WeaponType weaponType = WeaponType.Universal;
    public float reputationsAmount = 0;
    
    [Header("Level Progression")]
    public LevelUpgrade[] levelUpgrades;

    [Header("Weapon")]
    public Weapon currentWeapon;
}

[System.Serializable]
public class LevelUpgrade
{
    public int level;
    public float areaBonus;
    public float damageBonus;
    public float cooldownReduction;
    public float lifetimeBonus;
    public float speedBonus;
    public string upgradeDescription;
}