using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class EnemyHP : MonoBehaviour
{
    private float _maxHP;
    private GameObject _expPrefab;
    private float _expDropPercent;
    private float _expAutodropAmount;

    public EnemyConfig _initializedStats;
    [NonSerialized]public EnemyPool _pool;

    private PlayerEXP _playerEXP;
    private float _currentHP;
    private EnemyHitEffect hitEffect;

    public int EnemyType = 0;

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
        ExpOnDeath();
        if (_pool != null)
            _pool.GetEnemyBackToPool(gameObject);
        else 
            Destroy(gameObject);

        ItemControllerSO.Instance.ActivateOnEnemyDeathEvent(gameObject);
        QuestManager.Instance?.OnEnemyKilled(EnemyType);
        PlayerStatsSO.Instance.ChangeKills(1);
    }
    
    private void ExpOnDeath() 
    {
        if (_expPrefab != null)
            Instantiate(_expPrefab, new Vector3(gameObject.transform.position.x, 0.2f, gameObject.transform.position.z), Quaternion.identity);
        _playerEXP.GetEXP(_expAutodropAmount);
    }

    private GameObject getExpPrefab()
    {
        float[] probabilities = new float[] { _initializedStats._expSmallDropChance, _initializedStats._expMediumDropChance, _initializedStats._expHugeDropChance, _initializedStats._noExpDropChance };

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