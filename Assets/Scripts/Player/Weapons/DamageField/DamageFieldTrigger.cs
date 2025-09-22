using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFieldTrigger : Weapon
{
    protected override void Start()
    {
        // Устанавливаем базовые статы для этого конкретного оружия
        _weaponArea = 8f;
        _weaponDamage = 10f;
        _weaponCooldown = 1f;
        
        // Вызываем базовый Start для инициализации
        base.Start();
    }

    public override void AddLevel(int value)
    {
        for (; value > 0 && _currentLevel <= _maxLevel; value--)
        {
            _currentLevel++;
            switch (_currentLevel)
            {
                case 1: _weaponArea += 2; break;
                case 2: _weaponDamage += 2; break;
                case 3: _weaponCooldown -= 0.2f; break;
                case 4: _weaponArea += 2; break;
                case 5: _weaponDamage += 5; break;
                case 6: _weaponCooldown -= 0.2f; break;
            }
            
            // Пересчитываем статы после изменения базовых значений
            if (playerStats != null)
            {
                CountArea(playerStats.GetAreaMultiplier());
                CountCooldown(playerStats.GetCooldownReduction());
                CountDamage(playerStats.GetDamageMultiplier());
            }
        }
    }

    public override void ReduceLevel(int value)
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

            // Пересчитываем статы после изменения базовых значений
            if (playerStats != null)
            {
                CountArea(playerStats.GetAreaMultiplier());
                CountCooldown(playerStats.GetCooldownReduction());
                CountDamage(playerStats.GetDamageMultiplier());
            }
        }
    }

    protected override void CountArea(float statsValue)
    {
        area = _weaponArea * statsValue;
        gameObject.transform.localScale = new Vector3(area, 0.05f, area);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHP>())
            gameObject.GetComponent<DamageField>().enabled = true;
    }

    public override string GetTextTitleInfo()
    {
        return "Оружие №2";
    }

    public override string GetTextDescriptionInfo()
    {
        return "Описание Оружия №2";
    }
}