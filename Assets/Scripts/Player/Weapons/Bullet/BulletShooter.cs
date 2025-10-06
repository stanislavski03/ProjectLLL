using System.Collections;
using UnityEngine;
using System;

public class BulletShooter : Weapon
{
    [Header("Bullet Shooter References")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private EnemyDetector enemyDetector;

    // Компоненты для эффектов
    private AudioSource audioSource;
    private ParticleSystem shootParticles;

    // Bullet-specific fields
    private float currentBulletSpeed;
    private float currentBulletLifetime;
    private int currentDamageType;

    private Coroutine shootingCoroutine;
    private bool isShooting = false;

    public event Action<float> OnDamageChanged;

    // Свойства для удобного доступа к специфичным данным
    private BulletShooterDataSO BulletData => weaponData as BulletShooterDataSO;

    protected override void Awake()
    {
        // Получаем компоненты
        if (enemyDetector == null)
            enemyDetector = GetComponent<EnemyDetector>();

        audioSource = GetComponent<AudioSource>();
        shootParticles = GetComponentInChildren<ParticleSystem>();

        base.Awake();
    }

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();

        if (weaponData is not BulletShooterDataSO)
        {
            Debug.LogError($"WeaponData must be of type BulletShooterDataSO for {gameObject.name}", this);
            return;
        }

        // Инициализируем параметры пули
        currentBulletSpeed = BulletData.bulletSpeed;
        currentBulletLifetime = BulletData.bulletLifetime;
        currentDamageType = BulletData.damageType;
    }

    protected override void Start()
    {
        base.Start();
        StartShooting();
    }

    protected override void CalculateStats()
    {
        base.CalculateStats();
        currentBulletLifetime = BulletData.bulletLifetime;

        OnDamageChanged?.Invoke(currentDamage);
    }

    protected override void CalculateCooldown(float cooldownReduction)
    {
        base.CalculateCooldown(cooldownReduction);
    }

    protected override void CalculateDamage(float damageMultiplier)
    {
        base.CalculateDamage(damageMultiplier);
        OnDamageChanged?.Invoke(currentDamage);
    }

    private void StartShooting()
    {
        StopShooting();

        if (enemyDetector == null)
        {
            Debug.LogError("EnemyDetector not found!", this);
            return;
        }

        if (bulletSpawn == null)
        {
            Debug.LogError("BulletSpawn transform not assigned!", this);
            return;
        }

        isShooting = true;
        shootingCoroutine = StartCoroutine(ShootingRoutine());
    }

    private void StopShooting()
    {
        isShooting = false;
        if (shootingCoroutine != null)
        {
            StopCoroutine(shootingCoroutine);
            shootingCoroutine = null;
        }
    }

    private IEnumerator ShootingRoutine()
    {
        // Первый выстрел сразу, без ожидания
        ShootAtVisibleEnemies();

        while (isShooting)
        {
            yield return new WaitForSeconds(currentCooldown);
            ShootAtVisibleEnemies();
        }
    }

    private void ShootAtVisibleEnemies()
    {
        if (enemyDetector == null) return;

        Enemy[] visibleEnemies = enemyDetector.GetVisibleEnemies();
        if (visibleEnemies != null && visibleEnemies.Length > 0)
        {
            Enemy closestEnemy = enemyDetector.GetClosestEnemy();
            if (closestEnemy != null)
            {
                Shoot(closestEnemy.transform);
                PlayShootEffects();
            }
        }
    }

    private void Shoot(Transform target)
    {
        if (BulletPool.Instance == null)
        {
            Debug.LogWarning("BulletPool instance not found!");
            return;
        }

        GameObject bulletObj = BulletPool.Instance.GetBullet();
        if (bulletObj == null)
        {
            Debug.LogWarning("Failed to get bullet from pool!");
            return;
        }

        // Настраиваем пулю
        bulletObj.SetActive(true);
        bulletObj.transform.position = bulletSpawn.position;

        Vector3 direction = (target.position - bulletSpawn.position).normalized;
        bulletObj.transform.rotation = Quaternion.LookRotation(direction);

        // Получаем компонент пули и настраиваем
        Bullet bulletController = bulletObj.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.InitializeBullet(
                target,
                currentBulletSpeed,
                currentBulletLifetime,
                currentDamageType,
                currentDamage
            );
        }
        else
        {
            Debug.LogWarning("Bullet component not found on bullet object!");
            BulletPool.Instance.ReturnBullet(bulletObj);
        }
    }

    private void PlayShootEffects()
    {
        // Визуальные эффекты
        if (shootParticles != null)
            shootParticles.Play();

        // Звуковые эффекты
        if (audioSource != null && BulletData.shootSound != null)
            audioSource.PlayOneShot(BulletData.shootSound);
    }

    // Методы для изменения типа урона
    public void SetDamageType(int damageType)
    {
        currentDamageType = Mathf.Clamp(damageType, 0, 2);
    }

    public void CycleDamageType()
    {
        currentDamageType = (currentDamageType + 1) % 3;
    }


    // UI методы
    public override string GetTextTitleInfo()
    {
        return weaponData?.weaponName ?? "Bullet Shooter";
    }

    public override string GetTextDescriptionInfo()
    {
        if (this?.GetUpgradeDescriptionForNextLevel() != "")
        {
            return this?.GetUpgradeDescriptionForNextLevel();
        }
        return this?.GetItemDescription();
    }

    public float GetBulletSpeed() => currentBulletSpeed;
    public float GetBulletLifetime() => currentBulletLifetime;
    public int GetDamageType() => currentDamageType;

    public string GetDamageTypeString()
    {
        return currentDamageType switch
        {
            0 => "Freeze",
            1 => "Fire",
            2 => "Electro",
            _ => "Unknown"
        };
    }
}