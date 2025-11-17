using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

[CreateAssetMenu(fileName = "New TechnoDamageBoostItem Data", menuName = "Items/TechnoDamageBoostItem Data")]
public class TechnoDamageBoostItemSO : ItemDataSO
{
    public float DamageMultiplier;

    public override void OnPick()
    {
        base.OnPick();

        PlayerStatsSO.Instance.ChangeTechnoDamageMultiplier(DamageMultiplier);
    }
}
