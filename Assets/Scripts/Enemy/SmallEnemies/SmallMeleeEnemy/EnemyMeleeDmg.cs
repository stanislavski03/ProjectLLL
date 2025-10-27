using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeDmg : MonoBehaviour
{
    private float _damage;
    private float _damageCooldown;

    private float _cooldownTimer = 0;
    private NavMeshAgent _agent;
    private bool _canDamage = true;
    private bool isPaused;
    //private bool isDestroyed = false;

    [SerializeField] private EnemyConfig _initializedStats;
    private void OnEnable()
    {
        _agent = GetComponent<NavMeshAgent>();
        try
        {
            _damage = _initializedStats._damage;
            _damageCooldown = _initializedStats._cooldown;
        }
        catch { }
    }

    void OnDestroy()
    {
        //isDestroyed = true;
        StopAllCoroutines();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isPaused) return;
        if (_canDamage)
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
        _canDamage = false;
        float speed = _agent.speed;
        _agent.speed = 0;
        for (_cooldownTimer = _damageCooldown; _cooldownTimer > 0; _cooldownTimer -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _agent.speed = speed;
        _canDamage = true;
    }

    //public void ReturnToPool()
    //{
    //    EnemyPool.Instance.GetEnemyBackToPool(gameObject);
    //}

}
