using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    private DamageFieldTrigger damageFieldTrigger;
    private float damage;
    private float cooldown;
    private List<EnemyHP> targets = new List<EnemyHP>();
    private bool isActive = false;

    private void Awake()
    {
        damageFieldTrigger = GetComponent<DamageFieldTrigger>();
    }

    private void OnEnable()
    {
        // Получаем актуальные значения
        damage = damageFieldTrigger.GetDamage();
        cooldown = damageFieldTrigger.GetCooldown();

        if (!isActive)
        {
            isActive = true;
            StartCoroutine(Damager());
        }
    }

    private void OnDisable()
    {
        isActive = false;
        StopAllCoroutines();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && !targets.Contains(enemy))
        {
            targets.Add(enemy);

            // Автоматически включаем компонент при появлении врагов
            if (!isActive)
            {
                enabled = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && targets.Contains(enemy))
        {
            targets.Remove(enemy);

            // Выключаем компонент, если врагов нет
            if (targets.Count == 0)
            {
                enabled = false;
            }
        }
    }

    IEnumerator Damager()
    {
        while (isActive)
        {
            if (targets.Count == 0)
            {
                enabled = false;
                yield break;
            }

            // Создаем копию списка для безопасной итерации
            List<EnemyHP> currentTargets = new List<EnemyHP>(targets);

            for (int i = 0; i < currentTargets.Count; i++)
            {
                if (currentTargets[i] != null)
                {
                    currentTargets[i].Damage(damage);

                    // Удаляем мертвых врагов из основного списка
                    if (currentTargets[i].GetHP() <= 0)
                    {
                        targets.Remove(currentTargets[i]);
                    }
                }
            }

            yield return new WaitForSeconds(cooldown);

            // Обновляем значения на каждом цикле
            damage = damageFieldTrigger.GetDamage();
            cooldown = damageFieldTrigger.GetCooldown();
        }
    }
}