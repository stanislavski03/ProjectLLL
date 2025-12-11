using UnityEngine;

[CreateAssetMenu(fileName = "New UniversalDamageBoostItem Data", menuName = "Items/UniversalDamageBoostItem Data")]
public class UniversalDamageBoostItemSO : ItemDataSO
{
    public float DamageMultiplier;

    public override void OnPick()
    {
        base.OnPick();

        PlayerStatsSO.Instance.ChangeDamageMultiplier(DamageMultiplier);
    }
}
