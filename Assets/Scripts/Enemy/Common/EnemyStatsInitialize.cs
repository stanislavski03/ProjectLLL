using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyStatsInitialize : MonoBehaviour
{
    [SerializeField] private EnemyConfig config;
    
    public string _enemyName;
    public int _maxHealth;
    public int _damage;
    public float _damageCooldown;
    public float _moveSpeed;
    public int _experienceReward;
    public float _expOnDeath;

    public float _expSmallDropChance;
    public float _expMediumDropChance;
    public float _expHugeDropChance;
    public GameObject _expSmallPrefab;
    public GameObject _expMediumPrefab;
    public GameObject _expHugePrefab;

    public float _size;

    private void Awake()
    {
        _enemyName = config._enemyName;
        _maxHealth = config._maxHealth;
        _damage = config._damage;
        _damageCooldown = config._cooldown;
        _moveSpeed = config._moveSpeed;
        _experienceReward = config._experienceReward;
        _expOnDeath = config._expOnDeath;
        _expSmallDropChance = config._expSmallDropChance;
        _expMediumDropChance = config._expMediumDropChance;
        _expHugeDropChance = config._expHugeDropChance;
        _expSmallPrefab = config._expSmallPrefab;
        _expMediumPrefab = config._expMediumPrefab;
        _expHugePrefab = config._expHugePrefab;
        _size = config._size;
        transform.localScale *= _size;
    }

}
