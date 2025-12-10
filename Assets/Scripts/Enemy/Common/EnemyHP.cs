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

    [SerializeField] private EnemyConfig _initializedStats;
    [NonSerialized]public EnemyPool _pool;

    private PlayerEXP _playerEXP;
    private float _currentHP;
    private EnemyHitEffect hitEffect;

    public int EnemyType = 0;

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
        _expPrefab = _initializedStats._expSmallPrefab;
        _expDropPercent = _initializedStats._expSmallDropChance;
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
    }
    
    private void ExpOnDeath() 
    {
        if (UnityEngine.Random.Range(1f, 100f) <= _expDropPercent)
            Instantiate(_expPrefab, new Vector3(gameObject.transform.position.x, 0.2f, gameObject.transform.position.z), Quaternion.identity);
        _playerEXP.GetEXP(_expAutodropAmount);
    }
}