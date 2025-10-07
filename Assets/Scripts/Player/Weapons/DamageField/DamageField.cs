using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageField : MonoBehaviour
{
    [SerializeField] private Weapon weaponSource;
    private List<EnemyHP> targets = new List<EnemyHP>();
    private bool isActive = false;

    private void Awake()
    {
        if (weaponSource == null)
            weaponSource = GetComponentInParent<Weapon>();
    }

    public void SetWeaponSource(Weapon source)
    {
        weaponSource = source;
    }

    public void EnableDamageField()
    {
        if (!isActive && weaponSource != null)
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
        while (isActive && targets.Count > 0 && weaponSource != null)
        {
            float actualDamage = weaponSource.GetDamage();
            float actualCooldown = weaponSource.GetCooldown();


            for (int i = targets.Count - 1; i >= 0; i--)
            {
                if (targets[i] != null)
                {
                    targets[i].Damage(actualDamage);
                    
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

            yield return new WaitForSeconds(actualCooldown);
        }
        
        isActive = false;
    }
}