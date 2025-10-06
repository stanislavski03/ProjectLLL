using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    [SerializeField] private Weapon weaponSource;
    private List<EnemyHP> targets = new List<EnemyHP>();
    private bool isActive = false;
    private float currentDamage;
    private float currentCooldown;

    private void Awake()
    {
        if (weaponSource == null)
            weaponSource = GetComponentInParent<Weapon>();
    }

    public void UpdateStats(float damage, float cooldown)
    {
        currentDamage = damage;
        currentCooldown = cooldown;
    }

    public void EnableDamageField()
    {
        if (!isActive)
        {
            isActive = true;
            StartCoroutine(DamageCoroutine());
        }
    }

    public void DisableDamageField()
    {
        isActive = false;
        StopAllCoroutines();
        targets.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && !targets.Contains(enemy))
        {
            targets.Add(enemy);
            if (!isActive)
            {
                EnableDamageField();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null)
        {
            targets.Remove(enemy);
        }
    }

    private IEnumerator DamageCoroutine()
    {
        while (isActive && targets.Count > 0)
        {
            // Обновляем статы от оружия
            if (weaponSource != null)
            {
                currentDamage = weaponSource.GetDamage();
                currentCooldown = weaponSource.GetCooldown();
            }

            // Наносим урон всем целям
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (targets[i] != null)
                {
                    targets[i].Damage(currentDamage);
                    
                    if (targets[i].GetHP() <= 0)
                    {
                        targets.RemoveAt(i);
                    }
                }
                else
                {
                    targets.RemoveAt(i);
                }
            }

            yield return new WaitForSeconds(currentCooldown);
        }
        
        isActive = false;
    }
}