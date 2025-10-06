using UnityEngine;

public class DamageFieldTrigger : Weapon, IAreaWeapon
{
    [Header("Damage Field References")]
    [SerializeField] private DamageField damageField;

    private float currentArea;

    private DamageFieldDataSO DamageFieldData => weaponData as DamageFieldDataSO;

    public float GetArea() => currentArea;

    public void CalculateArea(float areaMultiplier)
    {
        float baseArea = GetBaseAreaForCurrentLevel();
        currentArea = baseArea * areaMultiplier;
        ApplyAreaToVisual();
    }

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
            Debug.LogError($"WeaponData must be of type DamageFieldDataSO for {gameObject.name}", this);
            return;
        }
    }

    protected virtual float GetBaseAreaForCurrentLevel()
    {
        if (DamageFieldData == null) return 0f;

        float area = DamageFieldData.baseArea;
        if (weaponData.levelUpgrades != null)
        {
            for (int i = 0; i < currentLevel && i < weaponData.levelUpgrades.Length; i++)
            {
                area += weaponData.levelUpgrades[i].areaBonus;
            }
        }
        return area;
    }

    protected virtual void ApplyAreaToVisual()
    {
        transform.localScale = new Vector3(currentArea, transform.localScale.y, currentArea);

        if (damageField != null)
        {
            damageField.UpdateStats(currentDamage, currentCooldown);
        }
    }

    protected override void CalculateStats()
    {
        base.CalculateStats();
        ApplyAreaToVisual();
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