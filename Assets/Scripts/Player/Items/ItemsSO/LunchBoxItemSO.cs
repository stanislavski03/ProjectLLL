using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New LunchBoxItemSO Data", menuName = "Items/LunchBoxItemSO Data")]

public class LunchBoxItemSO : ItemDataSO
{
    [SerializeField] private int HPAddition;
    public override void OnPick()
    {
        base.OnPick();
    }

    public override void OnLVLUp()
    {
        base.OnLVLUp();
        PlayerHP.Instance.Heal(HPAddition);
    }

}
