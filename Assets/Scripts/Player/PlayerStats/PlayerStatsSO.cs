using UnityEngine;
using System;

[CreateAssetMenu(fileName = "Player Stats", menuName = "Player Stats")]
public class PlayerStatsSO : SingletonScriptableObject<PlayerStatsSO>
{
    [Header("Basic Info")]
    public string playerName;
    
    [Header("Player Stats")]
    public float DamageMultiplier = 1;
    public float MagicDamageMultiplier = 1;
    public float TechnoDamageMultiplier = 1;
    public float CooldownReduction = 1;
    public float AreaMultiplier = 1;
    public float MaxHP = 100;
    
    public float MoveSpeed = 10;
    public float SpeedMultiplier = 1;
    public long Money = 0;
    public long MaxMoney = 0;
    public float Reputation = 50;

    public int maxEXP = 100;
    public int maxLevel = 30;

    public bool invincibility = false;

    public float CurrentKills = 0;
    
    public event Action<float> _damageMultiplierChanged;
    public event Action<float> _magicDamageMultiplierChanged;
    public event Action<float> _technoDamageMultiplierChanged;
    public event Action<float> _cooldownReductionChanged;
    public event Action<float> _areaMultiplierChanged;
    public event Action<float> _speedMultiplierChanged;
    public event Action<float> _moneyChanged;
    public event Action<float> _reputationChanged;
    public event Action<float> _moveSpeedChanged;
    public event Action<float> _maxHpChanged;
    public event Action<float> _killsChanged;
    //public event Action<float> _fireDamageMultiplierChanged;
    //public event Action<float> _iceDamageMultiplierChanged;
    //public event Action<float> _energyDamageMultiplierChanged;


    private void Awake()
    {
        UpdateAllPlayerStats();
    }

    public void ExpandEXP()
    {
        maxEXP += 150;
    }

    public void UpdateAllPlayerStats()
    {
        _damageMultiplierChanged?.Invoke(DamageMultiplier);
        _magicDamageMultiplierChanged?.Invoke(MagicDamageMultiplier);
        _technoDamageMultiplierChanged?.Invoke(TechnoDamageMultiplier);
        _cooldownReductionChanged?.Invoke(CooldownReduction);
        _areaMultiplierChanged?.Invoke(AreaMultiplier);
        _speedMultiplierChanged?.Invoke(SpeedMultiplier);
        _moneyChanged?.Invoke(Money);
        _reputationChanged?.Invoke(Reputation);
        _moveSpeedChanged?.Invoke(MoveSpeed);
        _maxHpChanged?.Invoke(MaxHP);
        _killsChanged?.Invoke(CurrentKills);
    }

    public void ChangeDamageMultiplier(float value)
    {
        DamageMultiplier += value;
        _damageMultiplierChanged?.Invoke(DamageMultiplier);
        Debug.Log("ChangeDamageMultiplier PlayerSO");
    }

    public void ChangeMagicDamageMultiplier(float value)
    {
        MagicDamageMultiplier += value;
        _magicDamageMultiplierChanged?.Invoke(MagicDamageMultiplier);
    }

    public void ChangeTechnoDamageMultiplier(float value)
    {
        TechnoDamageMultiplier += value;
        _technoDamageMultiplierChanged?.Invoke(TechnoDamageMultiplier);
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
        if(value > 0)
            MaxMoney += value;
        _moneyChanged?.Invoke(Money);
    }

    public void ChangeReputation(float value)
    {
        Reputation = Mathf.Clamp(Reputation + value, 0, 100);
        _reputationChanged?.Invoke(Reputation);
    }

    public void ChangeMoveSpeed(float value)
    {
        MoveSpeed += value;
        _moveSpeedChanged?.Invoke(MoveSpeed);
    }

    public void ChangeKills(long value)
    {
        CurrentKills += value;
        _killsChanged?.Invoke(CurrentKills);
    }

}