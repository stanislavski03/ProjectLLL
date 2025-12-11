using UnityEngine;

[CreateAssetMenu(fileName = "New MagicDamageBoostItem Data", menuName = "Items/MagicDamageBoostItem Data")]
public class MagicDamageBoostItemSO : ItemDataSO
{
    public float DamageMultiplier;

    public override void OnPick()
    {
        base.OnPick();

        PlayerStatsSO.Instance.ChangeMagicDamageMultiplier(DamageMultiplier);
    }
}
