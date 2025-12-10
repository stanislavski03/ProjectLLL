using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMeleeDmg : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private float _damage;
    private float _damageCooldown;

    private float _cooldownTimer = 0;
    private NavMeshAgent _agent;
    private bool _canDamage = true;
    private bool isPaused;
    private bool _isAttacking = false;
    private bool _isInCollisionWithPlayer = false;
    //private bool isDestroyed = false;

    [SerializeField] private EnemyConfig _initializedStats;
    private void OnEnable()
    {
        _canDamage = true;
        _isAttacking = false;

        _agent = GetComponent<NavMeshAgent>();
        try{
            animator = GetComponent<Animator>();
        }
        catch { }
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

    private void Update()
    {
        try
        {
            animator.SetBool("IsAttacking", _isAttacking);
        }
        catch { }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player))
            _isInCollisionWithPlayer = true;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (isPaused) return;
        if (_canDamage)
            MeleeAttack(collision);
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player))
            _isInCollisionWithPlayer = false;
    }

    private void MeleeAttack(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out PlayerHP player) && _cooldownTimer <= 0 && enabled)
        {
            try
            {
                player.Damage(_damage);
                StartCoroutine(DamageCooldown());
            }
            catch { }
        }
    }
    private IEnumerator DamageCooldown()
    {
        _canDamage = false;
        _isAttacking = true;
        float speed = _agent.speed;
        _agent.speed = 0;
        for (_cooldownTimer = _damageCooldown; _cooldownTimer > 0; _cooldownTimer -= 0.1f)
        {
            yield return new WaitForSeconds(0.1f);
        }
        _agent.speed = speed;
        _canDamage = true;
        if(!_isInCollisionWithPlayer)
            _isAttacking = false;
    }

    //public void ReturnToPool()
    //{
    //    EnemyPool.Instance.GetEnemyBackToPool(gameObject);
    //}

}
