using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHP : MonoBehaviour
{
    private float _maxHP;
    private GameObject _expPrefab;
    private GameObject _healSpherePrefab;
    private float _expDropPercent;
    private float _expAutodropAmount;

    public EnemyConfig _initializedStats;
    [NonSerialized] public EnemyPool _pool;

    private PlayerEXP _playerEXP;
    private float _currentHP;
    private EnemyHitEffect hitEffect;

    public int EnemyType = 0;

    // Префаб с партикл эффектом смерти
    [SerializeField] private GameObject _deathParticlePrefab;
    
    // Смещение для спавна партикла
    [SerializeField] private Vector3 _particleOffset = new Vector3(0, 0.2f, 0);
    
    // Время жизни партикла после спавна
    [SerializeField] private float _particleLifetime = 2f;

    public event Action EnemyHPCnanged;

    private void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerEXP = player.GetComponent<PlayerEXP>();
        }

        // Получаем компонент эффекта
        hitEffect = GetComponent<EnemyHitEffect>();
    }

    private void OnEnable()
    {
        try
        {
            InitializeStats();
        }
        catch { }
    }

    private void InitializeStats()
    {
        _maxHP = _initializedStats._maxHealth;
        _expPrefab = getExpPrefab();
        _healSpherePrefab = _initializedStats._healSpherePrefab;
        _expAutodropAmount = _initializedStats._expOnDeath;
        _currentHP = _maxHP;
    }

    public float GetHP()
    {
        return _currentHP;
    }

    public float GetMaxHP()
    {
        return _maxHP;
    }

    public void Damage(float damageAmount)
    {
        _currentHP -= damageAmount;
        EnemyHPCnanged?.Invoke();

        if (hitEffect != null && _currentHP > 0)
            hitEffect.TakeHit();

        if (_currentHP <= 0)
            Death();
    }

    public void Heal(float healAmount)
    {
        _currentHP += healAmount;
    }

    private void Death()
    {
        // Спавним партикл эффект смерти
        SpawnDeathParticle();
        
        ExpOnDeath();
        
        // Спавним хил сферу
        if (_healSpherePrefab != null)
        {
            if (UnityEngine.Random.Range(0, 100) <= 5)
                Instantiate(_healSpherePrefab, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.identity);
        }
        
        // Возвращаем в пул или уничтожаем
        if (_pool != null)
            _pool.GetEnemyBackToPool(gameObject);
        else
            Destroy(gameObject);

        // Вызываем события
        ItemControllerSO.Instance?.ActivateOnEnemyDeathEvent(gameObject);
        QuestManager.Instance?.OnEnemyKilled(EnemyType);
        PlayerStatsSO.Instance?.ChangeKills(1);
    }

    // Метод для спавна партикл эффекта смерти
    private void SpawnDeathParticle()
    {
        if (_deathParticlePrefab != null)
        {
            // Спавним префаб с партиклом
            GameObject deathParticle = Instantiate(
                _deathParticlePrefab, 
                transform.position + _particleOffset, 
                Quaternion.identity
            );
            
            // Автоматически уничтожаем через заданное время
            Destroy(deathParticle, _particleLifetime);
            
            // Если в префабе есть ParticleSystem, убедимся что он воспроизводится
            ParticleSystem particleSystem = deathParticle.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play();
            }
            else
            {
                // Проверяем в дочерних объектах
                particleSystem = deathParticle.GetComponentInChildren<ParticleSystem>();
                if (particleSystem != null)
                {
                    particleSystem.Play();
                }
            }
        }
    }

    // Альтернативный метод с кастомной логикой уничтожения
    private IEnumerator SpawnDeathParticleWithCoroutine()
    {
        if (_deathParticlePrefab != null)
        {
            GameObject deathParticle = Instantiate(
                _deathParticlePrefab, 
                transform.position + _particleOffset, 
                Quaternion.identity
            );
            
            // Ждем указанное время
            yield return new WaitForSeconds(_particleLifetime);
            
            // Уничтожаем если еще существует
            if (deathParticle != null)
            {
                Destroy(deathParticle);
            }
        }
    }

    private void ExpOnDeath()
    {
        // Спавним опыт
        if (_expPrefab != null)
            Instantiate(_expPrefab, new Vector3(transform.position.x, 0.2f, transform.position.z), Quaternion.identity);
        
        // Даем опыт игроку
        _playerEXP?.GetEXP(_expAutodropAmount);
    }

    private GameObject getExpPrefab()
    {
        float[] probabilities = new float[] { 
            _initializedStats._expSmallDropChance, 
            _initializedStats._expMediumDropChance, 
            _initializedStats._expHugeDropChance, 
            _initializedStats._noExpDropChance 
        };

        float totalProbability = 0f;
        for (int i = 0; i < probabilities.Length; i++)
        {
            totalProbability += probabilities[i];
        }

        float randomValue = UnityEngine.Random.Range(0f, totalProbability);
        float currentSum = 0f;

        for (int i = 0; i < probabilities.Length; i++)
        {
            currentSum += probabilities[i];
            if (randomValue <= currentSum)
            {
                // 0-Small, 1-Medium, 2-Large, 3-None
                switch (i)
                {
                    case 0: return _initializedStats._expSmallPrefab;
                    case 1: return _initializedStats._expMediumPrefab;
                    case 2: return _initializedStats._expHugePrefab;
                    case 3: return null;
                    default: return null;
                }
            }
        }
        return null;
    }
}