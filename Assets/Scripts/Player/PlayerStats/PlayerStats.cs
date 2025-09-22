using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private float _damageMultiplier = 1;
    [SerializeField] private float _cooldownReduction = 1;
    [SerializeField] private float _areaMultiplier = 1;
    [SerializeField] private float _fireDamageMultiplier = 1;
    [SerializeField] private float _iceDamageMultiplier = 1;
    [SerializeField] private float _energyDamageMultiplier = 1;

    public event Action<float> _damageMultiplierChanged;
    public event Action<float> _cooldownReductionChanged;
    public event Action<float> _areaMultiplierChanged;
    public event Action<float> _fireDamageMultiplierChanged;
    public event Action<float> _iceDamageMultiplierChanged;
    public event Action<float> _energyDamageMultiplierChanged;

    public float GetDamageMultiplier() => _damageMultiplier;
    public float GetCooldownReduction() => _cooldownReduction;
    public float GetAreaMultiplier() => _areaMultiplier;
    public float GetFireDamageMultiplier() => _fireDamageMultiplier;
    public float GetIceDamageMultiplier() => _iceDamageMultiplier;
    public float GetEnergyDamageMultiplier() => _energyDamageMultiplier;

    private void Awake()
    {
        _damageMultiplierChanged?.Invoke(_damageMultiplier);
        _cooldownReductionChanged?.Invoke(_cooldownReduction);
        _areaMultiplierChanged?.Invoke(_areaMultiplier);
        _fireDamageMultiplierChanged?.Invoke(_fireDamageMultiplier);
        _iceDamageMultiplierChanged?.Invoke(_iceDamageMultiplier);
        _energyDamageMultiplierChanged?.Invoke(_energyDamageMultiplier);
    }

    //����� ��� ������, ������� ��� �������

    private void Update()
    {
        _damageMultiplierChanged?.Invoke(_damageMultiplier);
        _cooldownReductionChanged?.Invoke(_cooldownReduction);
        _areaMultiplierChanged?.Invoke(_areaMultiplier);
        _fireDamageMultiplierChanged?.Invoke(_fireDamageMultiplier);
        _iceDamageMultiplierChanged?.Invoke(_iceDamageMultiplier);
        _energyDamageMultiplierChanged?.Invoke(_energyDamageMultiplier);
    }

    public void AddDamageMultiplier(float value)
    {
        _damageMultiplier += value;
        _damageMultiplierChanged?.Invoke(_damageMultiplier);
    }

    public void AddCooldownReduction(float value)
    {
        _cooldownReduction -= value;
        _cooldownReductionChanged?.Invoke(_cooldownReduction);
    }

    public void AddAreaMultiplier(float value)
    {
        _areaMultiplier += value;
        _areaMultiplierChanged?.Invoke(_areaMultiplier);
    }

    public void AddFireDamageMultiplier(float value)
    {
        _fireDamageMultiplier += value;
        _fireDamageMultiplierChanged?.Invoke(_fireDamageMultiplier);
    }

    public void AddIceDamageMultiplier(float value)
    {
        _iceDamageMultiplier += value;
        _iceDamageMultiplierChanged?.Invoke(_iceDamageMultiplier);
    }

    public void AddEnergyDamageMultiplier(float value)
    {
        _energyDamageMultiplier += value;
        _energyDamageMultiplierChanged?.Invoke(_energyDamageMultiplier);
    }

}
