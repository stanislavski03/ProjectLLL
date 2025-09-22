using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    [SerializeField] protected PlayerStats playerStats;
    
    // Базовые статы оружия (должны быть установлены в производных классах)
    protected float _weaponArea;
    protected float _weaponDamage;
    protected float _weaponCooldown;
    
    // Текущие рассчитанные статы
    protected float area;
    protected float damage;
    protected float cooldown;
    protected float additionalDamage = 1f;
    
    protected int _currentLevel = 0;
    protected int _maxLevel = 6;

    public abstract string GetTextTitleInfo();
    public abstract string GetTextDescriptionInfo();
    
    // Виртуальные методы для переопределения в производных классах
    public virtual void AddLevel(int value)
    {
        // Базовая реализация может быть пустой
    }
    
    public virtual void ReduceLevel(int value)
    {
        // Базовая реализация может быть пустой
    }

    protected virtual void Awake()
    {
        // Базовая инициализация в Awake
        playerStats = FindObjectOfType<PlayerStats>(); // или другой способ получения ссылки
    }

    protected virtual void Start()
    {
        // Инициализация статов при старте
        if (playerStats != null)
        {
            CountArea(playerStats.GetAreaMultiplier());
            CountCooldown(playerStats.GetCooldownReduction());
            CountAdditionalDamage(playerStats.GetEnergyDamageMultiplier());
            CountDamage(playerStats.GetDamageMultiplier());
        }
    }

    private void OnEnable()
    {
        if (playerStats != null)
        {
            playerStats._areaMultiplierChanged += CountArea;
            playerStats._cooldownReductionChanged += CountCooldown;
            playerStats._damageMultiplierChanged += CountDamage;
            playerStats._energyDamageMultiplierChanged += CountAdditionalDamage;
        }
    }

    private void OnDisable()
    {
        if (playerStats != null)
        {
            playerStats._areaMultiplierChanged -= CountArea;
            playerStats._cooldownReductionChanged -= CountCooldown;
            playerStats._damageMultiplierChanged -= CountDamage;
            playerStats._energyDamageMultiplierChanged -= CountAdditionalDamage;
        }
    }

    protected virtual void CountArea(float statsValue)
    {
        
    }

    protected virtual void CountCooldown(float statsValue)
    {
        cooldown = _weaponCooldown * statsValue;
    }

    protected virtual void CountDamage(float statsValue)
    {
        damage = _weaponDamage * statsValue * additionalDamage;
    }

    protected virtual void CountAdditionalDamage(float statsValue)
    {
        additionalDamage = statsValue;
        // Пересчитываем damage при изменении additionalDamage
        if (playerStats != null)
        {
            CountDamage(playerStats.GetDamageMultiplier());
        }
    }

    public float GetArea() => area;
    public float GetCooldown() => cooldown;
    public float GetDamage() => damage;
    public int GetCurrentLevel() => _currentLevel;
}