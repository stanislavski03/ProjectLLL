using System;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    public PlayerStatsSO playerStats;
    public WeaponDataSO weaponData;

    protected float currentDamage;
    protected float currentCooldown;
    public int currentLevel = 0;

    public bool IsAvailable = true;
    public Action AvailableChanged;

    public WeaponDataSO Data => weaponData;
    public PlayerStatsSO PlayerStatsData => playerStats;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel >= weaponData.maxLevel;

    public System.Action<Weapon> OnLevelUp;

    private void Update()
    {
        UpdatePlayerStats();
    }

    protected virtual void Awake()
    {
        InitializeWeapon();
    }

    protected virtual void InitializeWeapon()
    {
        if (weaponData == null)
        {
            return;
        }
    }

    protected virtual void SubscribeToPlayerStats()
    {
        if (playerStats != null)
        {
            playerStats._cooldownReductionChanged += OnCooldownReductionChanged;
            playerStats._damageMultiplierChanged += OnDamageMultiplierChanged;
        }
    }

    protected virtual void UnsubscribeFromPlayerStats()
    {
        if (playerStats != null)
        {
            playerStats._cooldownReductionChanged -= OnCooldownReductionChanged;
            playerStats._damageMultiplierChanged -= OnDamageMultiplierChanged;
        }
    }

    private void OnEnable()
    {
        SubscribeToPlayerStats();
        CalculateAllStats();
    }

    private void OnDisable()
    {
        UnsubscribeFromPlayerStats();
    }

    private void OnCooldownReductionChanged(float cooldownReduction)
    {
        CalculateAllStats();
    }

    private void OnDamageMultiplierChanged(float damageMultiplier)
    {
        CalculateAllStats();
    }

    // ГЛАВНЫЙ МЕТОД: РАСЧЕТ ВСЕХ СТАТОВ
    protected virtual void CalculateAllStats()
    {
        CalculateDamage();
        CalculateCooldown();
    }

    // РАСЧЕТ УРОНА: базовый урон + бонусы уровня + множитель игрока
    protected virtual void CalculateDamage()
    {
        float baseDamage = weaponData.baseDamage;
        
        // ДОБАВЛЯЕМ БОНУСЫ ОТ УРОВНЕЙ
        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < weaponData.levelUpgrades.Length; i++)
            {
                var upgrade = weaponData.levelUpgrades[i];
                if (upgrade.level <= currentLevel)
                {
                    baseDamage += upgrade.damageBonus;
                }
            }
        }
        
        // ПРИМЕНЯЕМ МНОЖИТЕЛЬ ИГРОКА
        float damageMultiplier = playerStats.DamageMultiplier;
        currentDamage = baseDamage * damageMultiplier;
        
    }

    protected virtual void CalculateCooldown()
    {
        float baseCooldown = weaponData.baseCooldown;

        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < weaponData.levelUpgrades.Length; i++)
            {
                var upgrade = weaponData.levelUpgrades[i];
                if (upgrade.level <= currentLevel)
                {
                    baseCooldown -= upgrade.cooldownReduction;
                }
            }
        }

        baseCooldown = Mathf.Max(baseCooldown, 0.05f);

        // ПРИМЕНЯЕМ МНОЖИТЕЛЬ ИГРОКА
        float cooldownMultiplier = playerStats.CooldownReduction;
        currentCooldown = baseCooldown * cooldownMultiplier;
    }

    public virtual bool CanLevelUp()
    {
        return currentLevel < weaponData.maxLevel;
    }

    public virtual void AddLevel(int levels = 1)
    {
        if (currentLevel == 0)
        {
            this.gameObject.SetActive(true);
        }

        int levelsAdded = 0;
        for (int i = 0; i < levels && CanLevelUp(); i++)
        {
            currentLevel++;
            levelsAdded++;
        }

        if (levelsAdded > 0)
        {
            CalculateAllStats();
            OnLevelUp?.Invoke(this);
        }
    }

    public virtual void ReduceLevel(int levels = 1)
    {
        int newLevel = Mathf.Max(0, currentLevel - levels);
        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            CalculateAllStats();
        }
    }

    public abstract string GetTextTitleInfo();
    public abstract string GetTextDescriptionInfo();

    public virtual string GetUpgradeDescriptionForNextLevel()
    {
        if (!CanLevelUp()) return "Max Level";

        int nextLevelIndex = currentLevel;
        if (weaponData.levelUpgrades != null && nextLevelIndex < weaponData.levelUpgrades.Length)
        {
            return weaponData.levelUpgrades[nextLevelIndex].upgradeDescription;
        }
        return "Upgrade available";
    }

    public virtual string GetItemDescription()
    {
        return weaponData.description;
    }

    public virtual float GetCooldown() => currentCooldown;
    public virtual float GetDamage() => currentDamage;
    public virtual float GetArea() => 0f;
    public virtual float GetProjectileSpeed() => 0f;
    public virtual float GetLifetime() => 0f;
    public virtual int GetDamageType() => 0;

    public virtual string GetWeaponStats()
    {
        string statsString = $"Cooldown: {GetCooldown()}\nDamage: {GetDamage()}\n";
        return statsString;
    }

    public void SetAvailable(bool available)
    {
        IsAvailable = available;
        AvailableChanged?.Invoke();
    }

    [ContextMenu("Test Add Level")]
    public void TestAddLevel()
    {
        AddLevel(1);
    }

    [ContextMenu("Debug Weapon Stats")]
    public void DebugWeaponStats()
    {
        Debug.Log($"=== {weaponData.weaponName} DEBUG ===");
        Debug.Log($"Level: {currentLevel}");
        Debug.Log($"Base Damage: {weaponData.baseDamage}");
        Debug.Log($"Base Cooldown: {weaponData.baseCooldown}");

        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < weaponData.levelUpgrades.Length; i++)
            {
                var upgrade = weaponData.levelUpgrades[i];
                Debug.Log($"Upgrade {i}: Level={upgrade.level}, DamageBonus={upgrade.damageBonus}, CooldownReduction={upgrade.cooldownReduction}");
            }
        }

        Debug.Log($"Current Damage: {currentDamage}");
        Debug.Log($"Current Cooldown: {currentCooldown}");
        Debug.Log($"=== END DEBUG ===");
    }
    
    private void UpdatePlayerStats()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            playerStats.AreaMultiplier -= 0.1f;
            playerStats.UpdateAllPlayerStats();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            playerStats.AreaMultiplier += 0.1f;
            playerStats.UpdateAllPlayerStats();
        }
    }

}