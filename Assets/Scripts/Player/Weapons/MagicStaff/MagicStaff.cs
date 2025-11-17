using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicStaff : Weapon
{
    [Header("Magic Staff References")]
    [SerializeField] private EnemyDetector enemyDetector;
    [SerializeField] private float maxExplosionArea = 10f;

    private float currentExplosionArea;
    private float currentExplosionLifetime;
    private float currentExplosionCooldown;
    private int currentMaxActiveExplosions;

    private Coroutine explosionCoroutine;
    private List<Explosion> activeExplosions = new List<Explosion>();
    private bool isActive = false;

    private MagicStaffDataSO MagicData => weaponData as MagicStaffDataSO;

    protected override void Awake()
    {
        if (enemyDetector == null)
            enemyDetector = GetComponent<EnemyDetector>();

        base.Awake();
    }

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();

        if (weaponData is not MagicStaffDataSO)
        {
            return;
        }

        CalculateExplosionStats();
    }

    private void Start()
    {
        StartExplosions();
    }

    protected override void SubscribeToPlayerStats()
    {
        base.SubscribeToPlayerStats();

        if (playerStats != null)
        {
            playerStats._areaMultiplierChanged += OnAreaMultiplierChanged;
            playerStats._damageMultiplierChanged += OnDamageMultiplierChanged;
            playerStats._magicDamageMultiplierChanged += OnDamageMultiplierChanged;
        }
    }

    protected override void UnsubscribeFromPlayerStats()
    {
        base.UnsubscribeFromPlayerStats();

        if (playerStats != null)
        {
            playerStats._areaMultiplierChanged -= OnAreaMultiplierChanged;
            playerStats._damageMultiplierChanged -= OnDamageMultiplierChanged;
            playerStats._magicDamageMultiplierChanged -= OnDamageMultiplierChanged;
        }
    }

    private void OnAreaMultiplierChanged(float areaMultiplier)
    {
        CalculateExplosionStats();
    }

    private void OnDamageMultiplierChanged(float DamageMultiplier)
    {
        CalculateDamage(DamageMultiplier);
    }

    protected override void CalculateDamage(float DamageMultiplier)
    {
        base.CalculateDamage();
        currentDamage *= DamageMultiplier;
        
    }

    protected override void CalculateAllStats()
    {
        base.CalculateAllStats();
        CalculateExplosionStats();

    }

    private void CalculateExplosionStats()
    {
        if (MagicData == null) return;

        currentExplosionArea = MagicData.explosionArea;
        currentExplosionLifetime = MagicData.explosionLifetime;
        currentExplosionCooldown = MagicData.explosionCooldown;

        if (weaponData.levelUpgrades != null && currentLevel > 0)
        {
            for (int i = 0; i < weaponData.levelUpgrades.Length; i++)
            {
                var upgrade = weaponData.levelUpgrades[i];
                if (upgrade.level <= currentLevel)
                {
                    currentExplosionArea += upgrade.areaBonus;
                    currentExplosionLifetime += upgrade.lifetimeBonus;
                    currentExplosionCooldown -= upgrade.cooldownReduction;
                }
            }
        }

        float areaMultiplier = playerStats.AreaMultiplier;
        currentExplosionArea *= areaMultiplier;

        // ОГРАНИЧИВАЕМ МАКСИМАЛЬНЫЙ РАЗМЕР
        currentExplosionArea = Mathf.Min(currentExplosionArea, maxExplosionArea);

        currentExplosionCooldown = Mathf.Max(currentExplosionCooldown, 0.1f);
        currentMaxActiveExplosions = Mathf.Max(currentMaxActiveExplosions, 1);

    }

    private void StartExplosions()
    {
        StopExplosions();

        if (enemyDetector == null)
        {
            return;
        }

        isActive = true;
        explosionCoroutine = StartCoroutine(ExplosionRoutine());
    }

    private void StopExplosions()
    {
        isActive = false;
        if (explosionCoroutine != null)
        {
            StopCoroutine(explosionCoroutine);
            explosionCoroutine = null;
        }
    }

    private IEnumerator ExplosionRoutine()
    {
        yield return new WaitForSeconds(1f);

        while (isActive)
        {
            Enemy closestEnemy = enemyDetector?.GetClosestEnemy();
            if (closestEnemy != null)
            {
                CreateExplosionOnClosestEnemy();
            }
            yield return new WaitForSeconds(currentCooldown);
        }
    }

    private void CreateExplosionOnClosestEnemy()
    {
        Enemy closestEnemy = enemyDetector?.GetClosestEnemy();
        if (closestEnemy == null) return;

        Vector3 explosionPosition = closestEnemy.transform.position;
        explosionPosition.y = 0f;

        if (ExplosionPool.Instance != null)
        {
            GameObject explosionObj = ExplosionPool.Instance.GetExplosion();
            if (explosionObj != null)
            {
                explosionObj.transform.position = explosionPosition;

                Explosion explosion = explosionObj.GetComponent<Explosion>();
                if (explosion != null)
                {
                    float baseDamage = weaponData.baseDamage;
                    explosion.InitializeExplosion(
                        currentExplosionArea,
                        currentExplosionLifetime,
                        baseDamage,
                        this
                    );
                }
            }
        }
    }

    public void OnExplosionFinished(Explosion explosion)
    {
        activeExplosions.Remove(explosion);
    }

    public override float GetArea() => currentExplosionArea;
    public override float GetLifetime() => currentExplosionLifetime;
    public override float GetCooldown() => currentExplosionCooldown;

    public override string GetWeaponStats()
    {
        string baseStats = base.GetWeaponStats();
        string statsString = baseStats +
            $"Explosion Area: {GetArea()}\n" +
            $"Explosion Lifetime: {GetLifetime()}\n" +
            $"Explosion Cooldown: {GetCooldown()}\n" +
            $"Max Active Explosions: {currentMaxActiveExplosions}";
        return statsString;
    }

    public override string GetTextTitleInfo()
    {
        return weaponData?.weaponName ?? "Magic Staff";
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
        StopExplosions();
        activeExplosions.Clear();
    }
}