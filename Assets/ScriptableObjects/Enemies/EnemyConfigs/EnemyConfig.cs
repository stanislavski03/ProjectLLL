using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Config", menuName = "Enemy/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [Header("Basic Stats")]
    public string _enemyName = "Enemy";
    public int _damage = 10;
    public float _cooldown = 1;
    public int _maxHealth = 100;
    public float _moveSpeed = 3.5f;
    public float _moveSpeedDeviation = 0.1f;
    public float _expOnDeath = 10;
    public float _size = 1;

    [Header("RangeAttack")]
    public float _range = 10;
    public float _projectileSpeed = 5;
    public float _projectileLifetime = 3;
    public float _prepareTime = 3;



    [Header("Experience")]
    public int _experienceReward = 50;
    [SerializeField] public GameObject _expSmallPrefab;
    public float _expSmallDropChance = 50;
    [SerializeField] public GameObject _expMediumPrefab;
    public float _expMediumDropChance = 0;
    [SerializeField] public GameObject _expHugePrefab;
    public float _expHugeDropChance = 0;

   
}