using System.Collections;
using UnityEngine;
using System;

public class BulletShooter: Weapon, IGameplaySystem
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

    private bool isPaused;

    private void Awake()
    {
        _enemyDetector = GetComponent<EnemyDetector>();
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


    public override string GetTextTitleInfo()
    {
        return "Оружие №1";
    }



    public float GetDamage(float damage)
    {
        return damage;
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        
        if (paused)
        {
            if (_shootingCoroutine != null)
                StopCoroutine(_shootingCoroutine);
            enabled = false;
        }
        else
        {
            enabled = true;
            StartShooting();
        }
    }
}