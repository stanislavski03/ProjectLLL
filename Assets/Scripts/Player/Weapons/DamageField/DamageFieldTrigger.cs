using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFieldTrigger : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private float _weaponCooldown = 1;
    [SerializeField] private float _weaponDamage = 10;
    [SerializeField] private float _weaponArea = 8;

    private float area;
    private float damage;
    private float cooldown;
    private float additionalDamage = 1f; // Инициализируем значением по умолчанию

    private int _currentLevel = 0;
    private int _maxLevel = 6;
    public int GetCurrentLevel() => _currentLevel;

    

    public void AddLevel(int value)
    {
        for (; value > 0 && _currentLevel <= _maxLevel; value--)
        {
            _currentLevel++;
            switch (_currentLevel) { 
                case 1: _weaponArea += 2; break;
                case 2: _weaponDamage += 2; break;
                case 3: _weaponCooldown -= 0.2f; break;
                case 4: _weaponArea += 2; break;
                case 5: _weaponDamage += 5; break;
                case 6: _weaponCooldown -= 0.2f; break;
            }
        }
    }

    public void ReduceLevel(int value)
    {
        for (; value > 0 && _currentLevel > 0; value--)
        {
            switch (_currentLevel)
            {
                case 1: _weaponArea -= 2; break;
                case 2: _weaponDamage -= 2; break;
                case 3: _weaponCooldown += 0.2f; break;
                case 4: _weaponArea -= 2; break;
                case 5: _weaponDamage -= 5; break;
                case 6: _weaponCooldown += 0.2f; break;
            }
            _currentLevel--;

        }
    }


    private void Start()
    {
        // Инициализируем начальные значения
        CountArea(playerStats.GetAreaMultiplier());
        CountCooldown(playerStats.GetCooldownReduction());
        CountAdditionalDamage(playerStats.GetEnergyDamageMultiplier());
        CountDamage(playerStats.GetDamageMultiplier());
    }



    private void OnEnable()
    {
        playerStats._areaMultiplierChanged += CountArea;
        playerStats._cooldownReductionChanged += CountCooldown;
        playerStats._damageMultiplierChanged += CountDamage;
        playerStats._energyDamageMultiplierChanged += CountAdditionalDamage;
    }

    private void OnDisable()
    {
        playerStats._areaMultiplierChanged -= CountArea;
        playerStats._cooldownReductionChanged -= CountCooldown;
        playerStats._damageMultiplierChanged -= CountDamage;
        playerStats._energyDamageMultiplierChanged -= CountAdditionalDamage;
    }



    private void CountArea(float statsValue)
    {
        area = _weaponArea * statsValue;
        gameObject.transform.localScale = new Vector3(area, 0.05f, area);
    }

    private void CountCooldown(float statsValue)
    {
        cooldown = _weaponCooldown * statsValue;
    }

    private void CountDamage(float statsValue)
    {
        damage = _weaponDamage * statsValue * additionalDamage;
    }

    private void CountAdditionalDamage(float statsValue)
    {
        additionalDamage = statsValue;
        // Пересчитываем damage при изменении additionalDamage
        CountDamage(playerStats.GetDamageMultiplier());
    }

    public float GetArea()
    {
        return area;
    }

    public float GetCooldown()
    {
        return cooldown;
    }

    public float GetDamage()
    {
        return damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHP>())
            gameObject.GetComponent<DamageField>().enabled = true;
    }
}