using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : Weapon
{
    [Header("Laser References")]
    [SerializeField] private EnemyDetector enemyDetector;
    [SerializeField] private float maxLaserLength = 15f;
    [SerializeField] private float maxLaserArea = 3f;

    private float currentLaserLength;
    private float currentLaserLifetime;
    private float currentLaserArea;
    private Coroutine laserCoroutine;
    private List<LaserBeam> activeLasers = new List<LaserBeam>();
    private bool isActive = false;

    private LaserDataSO LaserData => weaponData as LaserDataSO;

    protected override void Awake()
    {
        if (enemyDetector == null)
            enemyDetector = GetComponent<EnemyDetector>();

        base.Awake();
    }

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();

        if (weaponData is not LaserDataSO)
        {
            Debug.LogError("Laser requires LaserDataSO!");
            return;
        }

        CalculateLaserStats();
    }

    private void Start()
    {
        StartLaser();
    }

    protected override void SubscribeToPlayerStats()
    {
        base.SubscribeToPlayerStats();

        if (playerStats != null)
        {
            playerStats._areaMultiplierChanged += OnAreaMultiplierChanged;
        }
    }

    protected override void UnsubscribeFromPlayerStats()
    {
        base.UnsubscribeFromPlayerStats();

        if (playerStats != null)
        {
            playerStats._areaMultiplierChanged -= OnAreaMultiplierChanged;
        }
    }

    private void OnAreaMultiplierChanged(float areaMultiplier)
    {
        CalculateLaserStats();
    }

    protected override void CalculateAllStats()
    {
        base.CalculateAllStats();
        CalculateLaserStats();
    }

    private void CalculateLaserStats()
    {
        if (LaserData == null) return;

        currentLaserLength = LaserData.laserLength;
        currentLaserLifetime = LaserData.laserLifetime;
        currentLaserArea = LaserData.laserArea;

        if (weaponData.levelUpgrades != null && currentLevel > 0)
        {
            for (int i = 0; i < weaponData.levelUpgrades.Length; i++)
            {
                var upgrade = weaponData.levelUpgrades[i];
                if (upgrade.level <= currentLevel)
                {
                    currentLaserLength += upgrade.areaBonus;
                    currentLaserLifetime += upgrade.lifetimeBonus;
                    currentLaserArea += upgrade.areaBonus * 0.1f;
                }
            }
        }

        float areaMultiplier = playerStats.AreaMultiplier;
        currentLaserLength *= areaMultiplier;
        currentLaserArea *= areaMultiplier;

        currentLaserLength = Mathf.Min(currentLaserLength, maxLaserLength);
        currentLaserArea = Mathf.Min(currentLaserArea, maxLaserArea);

        Debug.Log($"Laser Stats - Length: {currentLaserLength}, Area: {currentLaserArea}, Lifetime: {currentLaserLifetime}");
    }

    private void StartLaser()
    {
        StopLaser();

        if (enemyDetector == null)
        {
            Debug.LogError("Laser missing EnemyDetector!");
            return;
        }

        isActive = true;
        laserCoroutine = StartCoroutine(LaserRoutine());
    }

    private void StopLaser()
    {
        isActive = false;
        if (laserCoroutine != null)
        {
            StopCoroutine(laserCoroutine);
            laserCoroutine = null;
        }

        foreach (var laser in activeLasers)
        {
            if (laser != null)
            {
                // Возвращаем в пул вместо уничтожения
                if (LaserPool.Instance != null)
                {
                    LaserPool.Instance.ReturnLaser(laser);
                }
            }
        }
        activeLasers.Clear();
    }

    private IEnumerator LaserRoutine()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log($"Laser started: cooldown = {currentCooldown}");

        while (isActive)
        {
            CreateLaserBeam();
            yield return new WaitForSeconds(currentCooldown);
        }
    }

    private void CreateLaserBeam()
    {
        Enemy closestEnemy = enemyDetector?.GetClosestEnemy();
        if (closestEnemy == null) return;

        // Получаем луч из пула с материалом
        LaserBeam laserBeam = LaserPool.Instance.GetLaser(
            currentLaserLength,
            currentLaserLifetime,
            currentDamage,
            LaserData.laserMaterial, // Передаем материал вместо цвета
            currentLaserArea,
            this,
            transform.position,
            closestEnemy.transform.position
        );

        if (laserBeam != null)
        {
            activeLasers.Add(laserBeam);
            Debug.Log($"Laser beam created from pool - Length: {currentLaserLength}, Area: {currentLaserArea}");
        }
        else
        {
            Debug.LogError("Failed to get laser beam from pool!");
        }
    }

    public void OnLaserFinished(LaserBeam laserBeam)
    {
        activeLasers.Remove(laserBeam);
        
        if (laserBeam != null && LaserPool.Instance != null)
        {
            LaserPool.Instance.ReturnLaser(laserBeam);
        }
    }

    public override float GetArea() => currentLaserArea;
    public override float GetLifetime() => currentLaserLifetime;

    public override string GetWeaponStats()
    {
        string baseStats = base.GetWeaponStats();
        string statsString = baseStats +
            $"Laser Length: {currentLaserLength}\n" +
            $"Laser Area: {GetArea()}\n" +
            $"Laser Lifetime: {GetLifetime()}\n" +
            $"Active Lasers: {activeLasers.Count}";
        return statsString;
    }

    public override string GetTextTitleInfo()
    {
        return weaponData?.weaponName ?? "Laser";
    }

    public override string GetTextDescriptionInfo()
    {
        if (this?.GetUpgradeDescriptionForNextLevel() != "")
        {
            return this?.GetUpgradeDescriptionForNextLevel();
        }
        return this?.GetItemDescription();
    }

    private void OnDisable()
    {
        StopLaser();
    }
}