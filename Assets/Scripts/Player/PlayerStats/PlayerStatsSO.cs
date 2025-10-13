using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats")]
public class PlayerStatsSO : ScriptableObject
{
    [Header("Basic Info")]
    public string playerName;
    
    [Header("Player Stats")]
    public float DamageMultiplier = 1;
    public float CooldownReduction = 1;
    public float AreaMultiplier = 1;
    public float FireDamageMultiplier = 1;
    public float IceDamageMultiplier = 1;
    public float EnergyDamageMultiplier = 1;
    public int maxLevel = 30;
    
    public event Action<float> _damageMultiplierChanged;
    public event Action<float> _cooldownReductionChanged;
    public event Action<float> _areaMultiplierChanged;
    public event Action<float> _fireDamageMultiplierChanged;
    public event Action<float> _iceDamageMultiplierChanged;
    public event Action<float> _energyDamageMultiplierChanged;

    private void Awake()
    {
        UpdateAllPlayerStats();
    }

    public void UpdateAllPlayerStats()
    {
        _damageMultiplierChanged?.Invoke(DamageMultiplier);
        _cooldownReductionChanged?.Invoke(CooldownReduction);
        _areaMultiplierChanged?.Invoke(AreaMultiplier);
        _fireDamageMultiplierChanged?.Invoke(FireDamageMultiplier);
        _iceDamageMultiplierChanged?.Invoke(IceDamageMultiplier);
        _energyDamageMultiplierChanged?.Invoke(EnergyDamageMultiplier);
    }

    public void AddDamageMultiplier(float value)
    {
        DamageMultiplier += value;
        _damageMultiplierChanged?.Invoke(DamageMultiplier);
    }

    public void AddCooldownReduction(float value)
    {
        CooldownReduction -= value;
        _cooldownReductionChanged?.Invoke(CooldownReduction);
    }

    public void AddAreaMultiplier(float value)
    {
        AreaMultiplier += value;
        _areaMultiplierChanged?.Invoke(AreaMultiplier);
    }

    public void AddFireDamageMultiplier(float value)
    {
        FireDamageMultiplier += value;
        _fireDamageMultiplierChanged?.Invoke(FireDamageMultiplier);
    }

    public void AddIceDamageMultiplier(float value)
    {
        IceDamageMultiplier += value;
        _iceDamageMultiplierChanged?.Invoke(IceDamageMultiplier);
    }

    public void AddEnergyDamageMultiplier(float value)
    {
        EnergyDamageMultiplier += value;
        _energyDamageMultiplierChanged?.Invoke(EnergyDamageMultiplier);
    }
}