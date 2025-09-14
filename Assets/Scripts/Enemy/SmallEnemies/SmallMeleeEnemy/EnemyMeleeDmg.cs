using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeDmg : MonoBehaviour, IGameplaySystem
{
    [SerializeField] private float _damage = 10f;
    [SerializeField] private float _damageCooldown = 1f;

    private float _cooldownTimer = 0;

    private bool isPaused;
    private bool isDestroyed;

    private void Awake()
    {

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

    public void SetPaused(bool paused)
    {
        if (isDestroyed) return; // Не выполняем если объект уничтожен
        
        isPaused = paused;
        
        if (paused)
        {
            enabled = false;
        }
        else
        {
            enabled = true;
        }
    }
}
