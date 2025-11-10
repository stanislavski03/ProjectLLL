using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
    public float MaxHP = 100;

    //not realised
    public float MoveSpeed = 10;
    public float SpeedMultiplier = 1;
    public long Money = 0;

    public int maxEXP = 100;
    public int maxLevel = 30;
    
    public event Action<float> _damageMultiplierChanged;
    public event Action<float> _cooldownReductionChanged;
    public event Action<float> _areaMultiplierChanged;
    public event Action<float> _speedMultiplierChanged;
    public event Action<float> _moneyChanged;
    public event Action<float> _moveSpeedChanged;
    public event Action<float> _maxHpChanged;
    //public event Action<float> _fireDamageMultiplierChanged;
    //public event Action<float> _iceDamageMultiplierChanged;
    //public event Action<float> _energyDamageMultiplierChanged;

    private static PlayerStatsSO _instance;
    public static PlayerStatsSO Instance
    {
        get
        {
            if (_instance == null)
            {
                var guids = AssetDatabase.FindAssets("t:PlayerStatsSO");
                if (guids.Length > 0)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guids[0]);
                    _instance = AssetDatabase.LoadAssetAtPath<PlayerStatsSO>(path);
                }

                if (_instance == null)
                {
                    Debug.LogWarning("PlayerStatsSO not found in project");
                }
            }
            return _instance;
        }
    }

    private void Awake()
    {
        UpdateAllPlayerStats();
    }

    public void UpdateAllPlayerStats()
    {
        _damageMultiplierChanged?.Invoke(DamageMultiplier);
        _cooldownReductionChanged?.Invoke(CooldownReduction);
        _areaMultiplierChanged?.Invoke(AreaMultiplier);
        _speedMultiplierChanged?.Invoke(SpeedMultiplier);
        _moneyChanged?.Invoke(Money);
        _moveSpeedChanged?.Invoke(MoveSpeed);
        _maxHpChanged?.Invoke(MaxHP);
    }

    public void ChangeDamageMultiplier(float value)
    {
        DamageMultiplier += value;
        _damageMultiplierChanged?.Invoke(DamageMultiplier);
    }

    public void ChangeMaxHp(float value)
    {
        MaxHP += value;
        _maxHpChanged?.Invoke(MaxHP);
    }

    public void ChangeCooldownReduction(float value)
    {
        CooldownReduction -= value;
        _cooldownReductionChanged?.Invoke(CooldownReduction);
    }

    public void ChangeAreaMultiplier(float value)
    {
        AreaMultiplier += value;
        _areaMultiplierChanged?.Invoke(AreaMultiplier);
    }
    public void ChangeSpeedMultiplier(float value)
    {
        SpeedMultiplier += value;
        _speedMultiplierChanged?.Invoke(SpeedMultiplier);
    }
    public void ChangeMoney(long value)
    {
        Money += value;
        _moneyChanged?.Invoke(Money);
    }
    public void ChangeMoveSpeed(float value)
    {
        MoveSpeed += value;
        _moveSpeedChanged?.Invoke(MoveSpeed);
    }

}