using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;

public class EnemyHP : MonoBehaviour
{
    private float _maxHP;
    private float _freezeDef = 0;
    private float _fireDef = 0;
    private float _electroDef = 0;
    private GameObject _expPrefab;
    private float _expDropPercent;
    private float _expAutodropAmount;

    [SerializeField] private EnemyConfig _initializedStats;

    private PlayerEXP _playerEXP;
    private float _currentHP;

    public event Action<float> onDamage;



    private void Start()
    {
            

        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerEXP = player.GetComponent<PlayerEXP>();
        }
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
    public void Damage(float damageAmount, int damageType)
    {
        switch (damageType)
        {
            case 0:
                _currentHP -= damageAmount * ((100 - _freezeDef) / 100);
                break;
            case 1:
                _currentHP -= damageAmount * ((100 - _fireDef) / 100);
                break;
            case 2:
                _currentHP -= damageAmount * ((100 - _electroDef) / 100);
                break;
            default:
                _currentHP -= damageAmount;
                break;
        }
        if (_currentHP <= 0) Death();
        // Debug.Log(_currentHP);
        onDamage?.Invoke(damageAmount);
    }


    public void Damage(float damageAmmount)
    {
        _currentHP -= damageAmmount;
        if (_currentHP <= 0) Death();
    }

    public void Heal(float healAmmount)
    {
        _currentHP += healAmmount;
    }

    private void Death()
    {
        Debug.Log("HOLY, HE'S DEAD!");
        if (UnityEngine.Random.Range(1f, 100f) <= _expDropPercent)
            Instantiate(_expPrefab, gameObject.transform.position, Quaternion.identity);
        _playerEXP.GetEXP(_expAutodropAmount);
        Destroy(gameObject);
    }
}
