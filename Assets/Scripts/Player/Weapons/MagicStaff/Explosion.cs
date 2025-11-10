using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [Header("Explosion Settings")]
    [SerializeField] private ParticleSystem explosionEffect;
    [SerializeField] private SphereCollider damageCollider;
    [SerializeField] private float baseScale = 1f;

    private float currentArea;
    private float currentLifetime;
    private float currentDamage;
    private MagicStaff staffSource;

    private List<EnemyHP> damagedEnemies = new List<EnemyHP>();
    private bool isActive = false;
    private Coroutine lifecycleCoroutine;

    public bool IsActive => isActive;

    private void Awake()
    {
        if (damageCollider == null)
            damageCollider = GetComponent<SphereCollider>();

        if (explosionEffect == null)
            explosionEffect = GetComponentInChildren<ParticleSystem>();
    }

    public void InitializeExplosion(float area, float lifetime, float damage, MagicStaff source = null)
    {
        currentArea = area;
        currentLifetime = lifetime;
        currentDamage = damage;
        staffSource = source;

        SetupExplosion();

        if (gameObject.activeInHierarchy)
        {
            lifecycleCoroutine = StartCoroutine(ExplosionLifecycle());
        }
    }

    private void SetupExplosion()
    {

        float finalScale = baseScale * currentArea;
        transform.localScale = new Vector3(finalScale, 0.001f, finalScale);

        if (damageCollider != null)
        {
            damageCollider.radius = 0.5f;
        }

        if (explosionEffect != null)
        {
            var main = explosionEffect.main;
            main.startSize = currentArea;
            explosionEffect.Play();
        }

        isActive = true;
        damagedEnemies.Clear();
    }

    private IEnumerator ExplosionLifecycle()
    {
        float timer = 0f;

        while (timer < currentLifetime && isActive)
        {
            DamageEnemiesInRadius();
            timer += 0.5f;
            yield return new WaitForSeconds(0.5f);
        }

        FinishExplosion();
    }

    private void DamageEnemiesInRadius()
    {
        float radius = (currentArea * baseScale) * 0.5f;
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius);

        foreach (var hitCollider in hitColliders)
        {
            EnemyHP enemy = hitCollider.GetComponent<EnemyHP>();
            if (enemy != null)
            {
                float actualDamage = staffSource != null ? staffSource.GetDamage() : currentDamage;
                enemy.Damage(actualDamage);
            }
        }
    }

    private IEnumerator DamageCoroutine()
    {
        while (isActive)
        {
            DamageEnemiesInRadius();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && !damagedEnemies.Contains(enemy))
        {
            float actualDamage = staffSource != null ? staffSource.GetDamage() : currentDamage;
            enemy.Damage(actualDamage);
            damagedEnemies.Add(enemy);
        }
    }

    private void FinishExplosion()
    {
        isActive = false;

        if (explosionEffect != null)
        {
            explosionEffect.Stop();
        }

        staffSource?.OnExplosionFinished(this);

        if (ExplosionPool.Instance != null)
        {
            ExplosionPool.Instance.ReturnExplosion(gameObject);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        if (isActive && lifecycleCoroutine == null)
        {
            lifecycleCoroutine = StartCoroutine(ExplosionLifecycle());
        }
    }

    private void OnDisable()
    {
        if (lifecycleCoroutine != null)
        {
            StopCoroutine(lifecycleCoroutine);
            lifecycleCoroutine = null;
        }
        ResetExplosion();
    }

    private void ResetExplosion()
    {
        isActive = false;
        damagedEnemies.Clear();
        staffSource = null;

        if (explosionEffect != null)
        {
            explosionEffect.Stop();
            explosionEffect.Clear();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        float radius = (currentArea * baseScale) * 0.5f;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}