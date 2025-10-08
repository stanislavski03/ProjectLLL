using UnityEngine;

public class DamageFieldTrigger : Weapon
{
    [Header("Damage Field References")]
    [SerializeField] private DamageField damageField;

    private float currentArea;
    private DamageFieldDataSO DamageFieldData => weaponData as DamageFieldDataSO;

    public override float GetArea() => currentArea;

    protected override void Awake()
    {
        base.Awake();
        if (damageField == null)
            damageField = GetComponent<DamageField>();
    }

    protected override void InitializeWeapon()
    {
        base.InitializeWeapon();

        if (weaponData is not DamageFieldDataSO)
        {
            return;
        }
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
        CalculateArea();
    }

    private void CalculateArea()
    {
        if (DamageFieldData == null) return;

        float baseArea = DamageFieldData.baseArea;
        
        // ДОБАВЛЯЕМ БОНУСЫ ОТ УРОВНЕЙ
        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < weaponData.levelUpgrades.Length; i++)
            {
                var upgrade = weaponData.levelUpgrades[i];
                if (upgrade.level <= currentLevel)
                {
                    baseArea += upgrade.areaBonus;
                }
            }
        }
        
        // ПРИМЕНЯЕМ МНОЖИТЕЛЬ ИГРОКА
        float areaMultiplier = playerStats?.GetAreaMultiplier() ?? 1f;
        currentArea = baseArea * areaMultiplier;
        
        ApplyAreaToVisual();
        
    }

    protected override void CalculateAllStats()
    {
        base.CalculateAllStats();
        CalculateArea();
        
    }

    private void ApplyAreaToVisual()
    {
        transform.localScale = new Vector3(currentArea, transform.localScale.y, currentArea);
        
        if (damageField != null)
        {
            damageField.SetWeaponSource(this);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHP>())
        {
            damageField?.EnableDamageField();
        }
    }

    public override string GetTextTitleInfo()
    {
        return weaponData?.weaponName ?? "Damage Field";
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