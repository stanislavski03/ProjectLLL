using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDmg : MonoBehaviour
{
    private float _damage = 10;
    private float _damageCooldown = 1;

    private float _cooldownTimer = 0;

    private bool isPaused;
    private bool isDestroyed;

    [SerializeField] private EnemyConfig _initializedStats;
    private void OnEnable()
    {
        try
        {
            _damage = _initializedStats._damage;
            _damageCooldown = _initializedStats._cooldown;
        }
        catch { }
    }

    void OnDestroy()
    {
        isDestroyed = true;
        StopAllCoroutines();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isPaused) return;

        PlayerCheckAndDamage(collision);
    }

    private void PlayerCheckAndDamage(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player) && _cooldownTimer <= 0 && enabled)
        {
            player.Damage(_damage);
            StartCoroutine(DamageCooldown());
        }
    }
    private IEnumerator DamageCooldown()
    {
        for (_cooldownTimer = _damageCooldown; _cooldownTimer > 0; _cooldownTimer -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void ReturnToPool()
    {
        EnemyPool.Instance.GetEnemyBackToPool(gameObject);
    }

}
