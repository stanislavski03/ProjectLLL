using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinotaurLavaExplosion : MonoBehaviour
{

    [SerializeField] private float _explosionDamage;
    [SerializeField] private float _lavaDamage;
    [SerializeField] private float _explosionTime;
    [SerializeField] private float _lavaDamagePeriod;
    private float damageCooldown = 0;



    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (damageCooldown == 0)
            {
                StartCoroutine(Damage(other.GetComponent<PlayerHP>()));
            }
        }
    }

    private IEnumerator Damage(PlayerHP other)
    {
        damageCooldown = _lavaDamagePeriod;
        other.Damage(_lavaDamage);
        while (damageCooldown >= 0)
        {
            damageCooldown-=0.2f;
            yield return new WaitForSeconds(0.2f);
        }
        damageCooldown = 0;
    }

}
