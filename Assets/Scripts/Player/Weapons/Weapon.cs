using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected WeaponDataSO weaponData;
    [SerializeField] protected PlayerStats playerStats;

    // Общие статы для всех оружий
    protected float currentDamage;
    protected float currentCooldown;

    protected int currentLevel = 0;
    protected float additionalDamage = 1f;

    public bool IsAvailable = true;
    public Action AvailableChanged;

    // Свойства для доступа к данным
    public WeaponDataSO Data => weaponData;
    public int CurrentLevel => currentLevel;
    public bool IsMaxLevel => currentLevel >= weaponData.maxLevel;

    // События для уведомления об изменениях
    public System.Action<Weapon> OnLevelUp;
    public System.Action<Weapon> OnStatsChanged;

    protected virtual void Awake()
    {
        playerStats = FindObjectOfType<PlayerStats>();
        InitializeWeapon();
    }

    protected virtual void Start()
    {
        SubscribeToPlayerStats();
        CalculateStats();
    }
// для теста
    private void Update()
    {
        ChangeAlailable();
    }

    protected virtual void InitializeWeapon()
    {
        if (weaponData == null)
        {
            Debug.LogError("WeaponData not assigned!", this);
            return;
        }
    }

    protected virtual void SubscribeToPlayerStats()
    {
        if (playerStats != null)
        {
            playerStats._cooldownReductionChanged += CalculateCooldown;
            playerStats._damageMultiplierChanged += CalculateDamage;
            playerStats._energyDamageMultiplierChanged += CalculateAdditionalDamage;

            // Подписываемся на Area только если оружие его использует
            if (this is IAreaWeapon)
            {
                playerStats._areaMultiplierChanged += OnAreaMultiplierChanged;
            }
        }
    }

    protected virtual void UnsubscribeFromPlayerStats()
    {
        if (playerStats != null)
        {
            playerStats._cooldownReductionChanged -= CalculateCooldown;
            playerStats._damageMultiplierChanged -= CalculateDamage;
            playerStats._energyDamageMultiplierChanged -= CalculateAdditionalDamage;

            if (this is IAreaWeapon)
            {
                playerStats._areaMultiplierChanged -= OnAreaMultiplierChanged;
            }
        }
    }

    private void OnEnable()
    {
        SubscribeToPlayerStats();
        CalculateStats();
    }

    private void OnDisable()
    {
        UnsubscribeFromPlayerStats();
    }

    // Обработчик изменения множителя area
    private void OnAreaMultiplierChanged(float areaMultiplier)
    {
        if (this is IAreaWeapon areaWeapon)
        {
            areaWeapon.CalculateArea(areaMultiplier);
        }
    }

    // Основной метод расчета всех статов
    protected virtual void CalculateStats()
    {
        CalculateCooldown(playerStats?.GetCooldownReduction() ?? 1f);
        CalculateAdditionalDamage(playerStats?.GetEnergyDamageMultiplier() ?? 1f);
        CalculateDamage(playerStats?.GetDamageMultiplier() ?? 1f);

        // Area рассчитывается только в оружиях, которые его используют
        if (this is IAreaWeapon areaWeapon)
        {
            areaWeapon.CalculateArea(playerStats?.GetAreaMultiplier() ?? 1f);
        }

        OnStatsChanged?.Invoke(this);
    }

    protected virtual void CalculateCooldown(float cooldownReduction)
    {
        float baseCooldown = GetBaseCooldownForCurrentLevel();
        currentCooldown = Mathf.Max(baseCooldown * cooldownReduction, 0.1f);
    }

    protected virtual void CalculateDamage(float damageMultiplier)
    {
        float baseDamage = GetBaseDamageForCurrentLevel();
        currentDamage = baseDamage * damageMultiplier * additionalDamage;
    }

    protected virtual void CalculateAdditionalDamage(float energyDamageMultiplier)
    {
        additionalDamage = energyDamageMultiplier;
        CalculateDamage(playerStats?.GetDamageMultiplier() ?? 1f);
    }

    // Методы для получения базовых статов с учетом уровня
    protected virtual float GetBaseDamageForCurrentLevel()
    {
        float damage = weaponData.baseDamage;
        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < currentLevel && i < weaponData.levelUpgrades.Length; i++)
            {
                damage += weaponData.levelUpgrades[i].damageBonus;
            }
        }
        return damage;
    }

    protected virtual float GetBaseCooldownForCurrentLevel()
    {
        float cooldown = weaponData.baseCooldown;
        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < currentLevel && i < weaponData.levelUpgrades.Length; i++)
            {
                cooldown -= weaponData.levelUpgrades[i].cooldownReduction;
            }
        }
        return Mathf.Max(cooldown, 0.1f);
    }

    // Система уровней
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
            CalculateStats();
            OnLevelUp?.Invoke(this);
        }
    }

    public virtual void ReduceLevel(int levels = 1)
    {
        int newLevel = Mathf.Max(0, currentLevel - levels);
        if (newLevel != currentLevel)
        {
            currentLevel = newLevel;
            CalculateStats();
        }
    }

    // Абстрактные методы для UI
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

    public float GetCooldown() => currentCooldown;
    public float GetDamage() => currentDamage;

    public void SetAvailable(bool available)
    {
        IsAvailable = available;
        AvailableChanged?.Invoke();
        Debug.Log(IsAvailable);
    }

    public void ChangeAlailable()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
            SetAvailable(true);
        }
        if (Input.GetKeyDown(KeyCode.I))
        {
            SetAvailable(false);
        }
    }

}

public interface IAreaWeapon
{
    void CalculateArea(float areaMultiplier);
    float GetArea();
}