using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy Config", menuName = "Enemy/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [Header("Basic Stats")]
    public string _enemyName = "Enemy";
    public int _maxHealth = 100;
    public int _damage = 10;
    public float _damageCooldown = 1;
    public float _moveSpeed = 3.5f;
    public int _experienceReward = 50;
    public float _expOnDeath = 10;

    [SerializeField] public GameObject _expSmallPrefab;
    public float _expSmallDropChance = 50;
    [SerializeField] public GameObject _expMediumPrefab;
    public float _expMediumDropChance = 0;
    [SerializeField] public GameObject _expHugePrefab;
    public float _expHugeDropChance = 0;

    public float _size = 1;

}