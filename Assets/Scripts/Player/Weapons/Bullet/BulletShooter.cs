using System.Collections;
using UnityEngine;
using System;

public class BulletShooter: MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _shootSpeed = 10f;
    [SerializeField] private float _bulletSpawnCooldown = 0.5f;
    [SerializeField] private float _bulletLifetime = 3f;
    [SerializeField] private float _weaponDamage = 10;
    [SerializeField] private int _damageType = 0; // 0=freeze, 1=fire, 2=electro

    public event Action<float> _damageChanged;

    private EnemyDetector _enemyDetector;
    private Coroutine _shootingCoroutine;

    private float _bulletCooldown;
    private float _damage;

    private void Awake()
    {
        _enemyDetector = GetComponent<EnemyDetector>();
        GameStateManager.Instance.OnGameStateChanged += OnGameStateChanged;
        CountdownController.OnCountdownStarted += OnCountdownStarted;
        CountdownController.OnCountdownFinished += OnCountdownFinished;
    }

    private void Start()
    {
        CountCooldown(playerStats.GetCooldownReduction());
        CountDamage(playerStats.GetDamageMultiplier());

        StartShooting();
    }

    private void OnDisable()
    {
        playerStats._cooldownReductionChanged -= CountCooldown;
        playerStats._damageMultiplierChanged -= CountDamage;
        

        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);
    }

    private void OnEnable()
    {
        playerStats._cooldownReductionChanged += CountCooldown;
        playerStats._damageMultiplierChanged += CountDamage;

        StartShooting();
    }

    private void StartShooting()
    {
        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);

        _shootingCoroutine = StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            ShootAtVisibleEnemies();
            yield return new WaitForSeconds(_bulletCooldown);
        }
    }

    private void ShootAtVisibleEnemies()
    {
        Enemy[] visibleEnemies = _enemyDetector.GetVisibleEnemies();
        if (visibleEnemies != null && visibleEnemies.Length > 0)
        {
            Enemy closestEnemy = _enemyDetector.GetClosestEnemy();
            if (closestEnemy != null)
            {
                Shoot(closestEnemy.transform);
            }
        }
    }

    private void Shoot(Transform target)
    {
        if (BulletPool.Instance == null)
        {
            return;
        }

        GameObject bulletObj = BulletPool.Instance.GetBullet();
        if (bulletObj == null)
        {
            return;
        }
        bulletObj.SetActive(true);
        bulletObj.transform.position = _bulletSpawn.position;

        Vector3 direction = (target.position - _bulletSpawn.position).normalized;
        bulletObj.transform.rotation = Quaternion.LookRotation(direction);

        Bullet bulletController = bulletObj.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.ResetBullet(target, _shootSpeed, _bulletLifetime, _damageType);
        }
        else
        {
            BulletPool.Instance.ReturnBullet(bulletObj);
        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnGameStateChanged -= OnGameStateChanged;
        CountdownController.OnCountdownStarted -= OnCountdownStarted;
        CountdownController.OnCountdownFinished -= OnCountdownFinished;

        if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);
    }

    public void SetDamageType(int damageType)
    {
        _damageType = Mathf.Clamp(damageType, 0, 2);
    }


    private void CountCooldown(float statsValue) 
    {
        _bulletCooldown = _bulletSpawnCooldown * statsValue;
    }
    private void CountDamage(float statsValue)
    {
        _damage = _weaponDamage * statsValue;
        _damageChanged?.Invoke(_damage);
    }



    public float GetDamage(float damage)
    {
        return damage;
    }

    private void OnCountdownStarted()
    {
        enabled = false;
    }

    private void OnCountdownFinished()
    {
        if (GameStateManager.Instance.CurrentGameState == GameState.Gameplay)
        {
            enabled = true;
            StartShooting();
        }
    }

    private void OnGameStateChanged(GameState newGameState)
    {
        if (newGameState == GameState.Paused)
        {
            enabled = false;
            if (_shootingCoroutine != null)
            StopCoroutine(_shootingCoroutine);
        }
        else if (newGameState == GameState.Gameplay)
        {
            enabled = false;
        }
    }
}