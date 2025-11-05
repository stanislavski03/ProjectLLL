using System.Collections;
using UnityEngine;

public class BulletShooter : Weapon
{
    [Header("Bullet Shooter References")]
    [SerializeField] private Transform bulletSpawn;
    [SerializeField] private EnemyDetector enemyDetector;

    private float currentBulletSpeed;
    private float currentBulletLifetime;
    private int currentDamageType;

    private Coroutine shootingCoroutine;
    private bool isShooting = false;

    private BulletShooterDataSO BulletData => weaponData as BulletShooterDataSO;

    protected override void Awake()
    {
        if (enemyDetector == null)
            enemyDetector = GetComponent<EnemyDetector>();

        base.Awake();
    }

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();

        if (weaponData is not BulletShooterDataSO)
        {
            return;
        }

        currentBulletSpeed = BulletData.bulletSpeed;
        currentBulletLifetime = BulletData.bulletLifetime;
        currentDamageType = BulletData.damageType;
    }

    private void Start()
    {
        StartShooting();
    }

    protected override void CalculateAllStats()
    {
        base.CalculateAllStats();

        currentBulletSpeed = BulletData.bulletSpeed * (1f + (currentLevel * 0.05f));
        currentBulletLifetime = BulletData.bulletLifetime;

    }

    public override float GetBulletSpeed() => currentBulletSpeed;
    public override float GetBulletLifetime() => currentBulletLifetime;
    public override int GetDamageType() => currentDamageType;

    private void StartShooting()
    {
        StopShooting();

        if (enemyDetector == null)
        {
            return;
        }

        if (bulletSpawn == null)
        {
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
        bulletObj.transform.position = bulletSpawn.position;

        Vector3 direction = (target.position - bulletSpawn.position).normalized;
        bulletObj.transform.rotation = Quaternion.LookRotation(direction);

        Bullet bulletController = bulletObj.GetComponent<Bullet>();
        if (bulletController != null)
        {
            bulletController.InitializeBullet(
                target,
                currentBulletSpeed,
                currentBulletLifetime,
                currentDamageType,
                currentDamage,
                this
            );

        }
        else
        {
            BulletPool.Instance.ReturnBullet(bulletObj);
        }
    }

    public override string GetWeaponStats()
    {
        string baseStats = base.GetWeaponStats();
        string statsString = baseStats + $"Bullet Speed: {GetBulletSpeed()}\nBullet Lifetime: {GetBulletLifetime()}";
        return statsString;
    }

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
}