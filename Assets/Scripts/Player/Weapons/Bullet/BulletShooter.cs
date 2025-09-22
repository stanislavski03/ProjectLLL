using System.Collections;
using UnityEngine;
using System;

public class BulletShooter : Weapon, IGameplaySystem
{
    [SerializeField] private Transform _bulletSpawn;
    [SerializeField] private float _shootSpeed = 10f;
    [SerializeField] private float _bulletSpawnCooldown = 0.5f;
    [SerializeField] private float _bulletLifetime = 3f;
    [SerializeField] private int _damageType = 0; // 0=freeze, 1=fire, 2=electro

    private EnemyDetector _enemyDetector;
    private Coroutine _shootingCoroutine;
    private float _bulletCooldown;

    private bool isPaused;

    public event Action<float> _damageChanged;

    protected override void Awake()
    {
        base.Awake(); // Важно вызвать базовый Awake
        _enemyDetector = GetComponent<EnemyDetector>();
    }

    protected override void Start()
    {
        // Устанавливаем базовые статы для этого оружия
        _weaponArea = 0f; // Не используется в этом оружии
        _weaponDamage = 10f;
        _weaponCooldown = 0.5f;
        
        // Вызываем базовый Start для инициализации
        base.Start();
        
        StartShooting();
    }

    private void OnEnable()
    {
        StartShooting();
    }

    private void OnDisable()
    {
        StopShooting();
    }

    private void OnDestroy()
    {
        StopShooting();
    }

    private void StopShooting()
    {
        if (_shootingCoroutine != null)
        {
            StopCoroutine(_shootingCoroutine);
            _shootingCoroutine = null;
        }
    }

    // Переопределяем метод расчета кд для этого оружия
    protected override void CountCooldown(float statsValue)
    {
        cooldown = _weaponCooldown * statsValue;
        _bulletCooldown = cooldown;
    }

    // Переопределяем метод расчета урона для этого оружия
    protected override void CountDamage(float statsValue)
    {
        damage = _weaponDamage * statsValue * additionalDamage;
        _damageChanged?.Invoke(damage);
    }

    private void StartShooting()
    {
        StopShooting(); // Останавливаем предыдущую корутину

        // Проверяем, что все компоненты инициализированы
        if (_enemyDetector == null)
        {
            _enemyDetector = GetComponent<EnemyDetector>();
            if (_enemyDetector == null)
            {
                Debug.LogError("EnemyDetector not found on " + gameObject.name);
                return;
            }
        }

        if (isPaused) return;

        _shootingCoroutine = StartCoroutine(ShootingRoutine());
    }

    private IEnumerator ShootingRoutine()
    {
        while (true)
        {
            // Добавляем проверку на null перед вызовом
            if (_enemyDetector != null)
            {
                ShootAtVisibleEnemies();
            }
            yield return new WaitForSeconds(_bulletCooldown);
        }
    }

    private void ShootAtVisibleEnemies()
    {
        // Добавляем дополнительные проверки
        if (_enemyDetector == null)
        {
            Debug.LogWarning("EnemyDetector is null in ShootAtVisibleEnemies");
            return;
        }

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
            Debug.LogWarning("BulletPool instance is null");
            return;
        }

        GameObject bulletObj = BulletPool.Instance.GetBullet();
        if (bulletObj == null)
        {
            Debug.LogWarning("Failed to get bullet from pool");
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
            Debug.LogWarning("Bullet component not found on bullet object");
            BulletPool.Instance.ReturnBullet(bulletObj);
        }
    }

    public void SetDamageType(int damageType)
    {
        _damageType = Mathf.Clamp(damageType, 0, 2);
    }

    public override void AddLevel(int value)
    {
        for (; value > 0 && _currentLevel <= _maxLevel; value--)
        {
            _currentLevel++;
            switch (_currentLevel)
            {
                case 1: _weaponDamage += 2; break;
                case 2: _weaponCooldown -= 0.1f; break;
                case 3: _weaponDamage += 3; break;
                case 4: _weaponCooldown -= 0.1f; break;
                case 5: _weaponDamage += 5; break;
                case 6: _weaponCooldown -= 0.1f; break;
            }
            
            if (playerStats != null)
            {
                CountCooldown(playerStats.GetCooldownReduction());
                CountDamage(playerStats.GetDamageMultiplier());
            }
        }
    }

    public override void ReduceLevel(int value)
    {
        for (; value > 0 && _currentLevel > 0; value--)
        {
            switch (_currentLevel)
            {
                case 1: _weaponDamage -= 2; break;
                case 2: _weaponCooldown += 0.1f; break;
                case 3: _weaponDamage -= 3; break;
                case 4: _weaponCooldown += 0.1f; break;
                case 5: _weaponDamage -= 5; break;
                case 6: _weaponCooldown += 0.1f; break;
            }
            _currentLevel--;

            if (playerStats != null)
            {
                CountCooldown(playerStats.GetCooldownReduction());
                CountDamage(playerStats.GetDamageMultiplier());
            }
        }
    }

    public override string GetTextTitleInfo()
    {
        return "Оружие №1";
    }

    public override string GetTextDescriptionInfo()
    {
        return "Описание Оружия №1";
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
            StopShooting();
            enabled = false;
        }
        else
        {
            enabled = true;
            StartShooting();
        }
    }
}



// СТАРОЕ
// using System.Collections;
// using UnityEngine;
// using System;

// public class BulletShooter: Weapon, IGameplaySystem
// {
//     [SerializeField] private PlayerStats playerStats;
//     [SerializeField] private Transform _bulletSpawn;
//     [SerializeField] private float _shootSpeed = 10f;
//     [SerializeField] private float _bulletSpawnCooldown = 0.5f;
//     [SerializeField] private float _bulletLifetime = 3f;
//     [SerializeField] private float _weaponDamage = 10;
//     [SerializeField] private int _damageType = 0; // 0=freeze, 1=fire, 2=electro

//     private EnemyDetector _enemyDetector;
//     private Coroutine _shootingCoroutine;
//     private float _bulletCooldown;
//     private float _damage;

//     private bool isPaused;

//     public event Action<float> _damageChanged;

//     private void Awake()
//     {
//         _enemyDetector = GetComponent<EnemyDetector>();
//     }

//     private void Start()
//     {
//         CountCooldown(playerStats.GetCooldownReduction());
//         CountDamage(playerStats.GetDamageMultiplier());

//         StartShooting();
//     }

//     private void OnEnable()
//     {
//         playerStats._cooldownReductionChanged += CountCooldown;
//         playerStats._damageMultiplierChanged += CountDamage;

//         StartShooting();
//     }

//     private void OnDisable()
//     {
//         playerStats._cooldownReductionChanged -= CountCooldown;
//         playerStats._damageMultiplierChanged -= CountDamage;


//         if (_shootingCoroutine != null)
//             StopCoroutine(_shootingCoroutine);
//     }

//     private void OnDestroy()
//     {
//         if (_shootingCoroutine != null)
//             StopCoroutine(_shootingCoroutine);
//     }

//     private void StartShooting()
//     {
//         if (_shootingCoroutine != null)
//             StopCoroutine(_shootingCoroutine);

//         _shootingCoroutine = StartCoroutine(ShootingRoutine());
//     }

//     private IEnumerator ShootingRoutine()
//     {
//         while (true)
//         {
//             ShootAtVisibleEnemies();
//             yield return new WaitForSeconds(_bulletCooldown);
//         }
//     }

//     private void ShootAtVisibleEnemies()
//     {
//         Enemy[] visibleEnemies = _enemyDetector.GetVisibleEnemies();
//         if (visibleEnemies != null && visibleEnemies.Length > 0)
//         {
//             Enemy closestEnemy = _enemyDetector.GetClosestEnemy();
//             if (closestEnemy != null)
//             {
//                 Shoot(closestEnemy.transform);
//             }
//         }
//     }

//     private void Shoot(Transform target)
//     {
//         if (BulletPool.Instance == null)
//         {
//             return;
//         }

//         GameObject bulletObj = BulletPool.Instance.GetBullet();
//         if (bulletObj == null)
//         {
//             return;
//         }
//         bulletObj.SetActive(true);
//         bulletObj.transform.position = _bulletSpawn.position;

//         Vector3 direction = (target.position - _bulletSpawn.position).normalized;
//         bulletObj.transform.rotation = Quaternion.LookRotation(direction);

//         Bullet bulletController = bulletObj.GetComponent<Bullet>();
//         if (bulletController != null)
//         {
//             bulletController.ResetBullet(target, _shootSpeed, _bulletLifetime, _damageType);
//         }
//         else
//         {
//             BulletPool.Instance.ReturnBullet(bulletObj);
//         }
//     }

//     public void SetDamageType(int damageType)
//     {
//         _damageType = Mathf.Clamp(damageType, 0, 2);
//     }


//     private void CountCooldown(float statsValue) 
//     {
//         _bulletCooldown = _bulletSpawnCooldown * statsValue;
//     }
//     private void CountDamage(float statsValue)
//     {
//         _damage = _weaponDamage * statsValue;
//         _damageChanged?.Invoke(_damage);
//     }


//     public override string GetTextTitleInfo()
//     {
//         return "Оружие №1";
//     }

//     public override string GetTextDescriptionInfo()
//     {
//         return "Описание Оружия №1";
//     }



//     public float GetDamage(float damage)
//     {
//         return damage;
//     }

//     public void SetPaused(bool paused)
//     {
//         isPaused = paused;
        
//         if (paused)
//         {
//             if (_shootingCoroutine != null)
//                 StopCoroutine(_shootingCoroutine);
//             enabled = false;
//         }
//         else
//         {
//             enabled = true;
//             StartShooting();
//         }
//     }
// }