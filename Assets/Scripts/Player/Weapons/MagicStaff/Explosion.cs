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

    private HashSet<EnemyHP> enemiesInField = new HashSet<EnemyHP>();
    private bool isActive = false;
    private Coroutine lifecycleCoroutine;
    private Coroutine damageOverTimeCoroutine;

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
            damageOverTimeCoroutine = StartCoroutine(DamageOverTime());
        }
    }

    private void SetupExplosion()
    {
        float finalScale = baseScale * currentArea;
        transform.localScale = new Vector3(finalScale, 0.001f, finalScale);

        if (damageCollider != null)
        {
            damageCollider.radius = 0.5f;
            damageCollider.isTrigger = true;
        }

        if (explosionEffect != null)
        {
            var main = explosionEffect.main;
            main.startSize = currentArea;
            explosionEffect.Play();
        }

        isActive = true;
        enemiesInField.Clear();
    }

    private IEnumerator ExplosionLifecycle()
    {
        yield return new WaitForSeconds(currentLifetime);
        FinishExplosion();
    }

    private IEnumerator DamageOverTime()
    {
        float damageInterval = 0.5f; // Урон каждые 0.5 секунд
        
        while (isActive)
        {
            // Наносим урон всем врагам в поле
            foreach (var enemy in new List<EnemyHP>(enemiesInField))
            {
                if (enemy != null)
                {
                    float actualDamage = staffSource != null ? staffSource.GetDamage() : currentDamage;
                    enemy.Damage(actualDamage);
                }
            }
            
            yield return new WaitForSeconds(damageInterval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive) return;

        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && !enemiesInField.Contains(enemy))
        {
            enemiesInField.Add(enemy);
            
            // Наносим урон сразу при входе в поле
            float actualDamage = staffSource != null ? staffSource.GetDamage() : currentDamage;
            enemy.Damage(actualDamage);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!isActive) return;

        EnemyHP enemy = other.GetComponent<EnemyHP>();
        if (enemy != null && enemiesInField.Contains(enemy))
        {
            enemiesInField.Remove(enemy);
        }
    }

    private void FinishExplosion()
    {
        isActive = false;
        enemiesInField.Clear();

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
            damageOverTimeCoroutine = StartCoroutine(DamageOverTime());
        }
    }

    private void OnDisable()
    {
        if (lifecycleCoroutine != null)
        {
            StopCoroutine(lifecycleCoroutine);
            lifecycleCoroutine = null;
        }
        
        if (damageOverTimeCoroutine != null)
        {
            StopCoroutine(damageOverTimeCoroutine);
            damageOverTimeCoroutine = null;
        }
        
        ResetExplosion();
    }

    private void ResetExplosion()
    {
        isActive = false;
        enemiesInField.Clear();
        staffSource = null;

        if (explosionEffect != null)
        {
            explosionEffect.Stop();
            explosionEffect.Clear();
        }
    }
}