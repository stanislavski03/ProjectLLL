using UnityEngine;
[CreateAssetMenu(fileName = "New Mutation Damage Data", menuName = "Mutations/Mutation Damage Data")]
public class MutationDamageSO : MutationDataSO
{
    public override void OnPick()
    {
        PlayerStatsSO.Instance.ChangeDamageMultiplier(bonus);
    }

    public override void OnDelete()
    {
        PlayerStatsSO.Instance.ChangeDamageMultiplier(bonus * -1);
    }

}
