using UnityEngine;
[CreateAssetMenu(fileName = "New Mutation Cooldown Reduction Data", menuName = "Mutations/Mutation Cooldown Reduction Data")]
public class MutationCooldownReductionSO : MutationDataSO
{
    public override void OnPick()
    {
        PlayerStatsSO.Instance.ChangeCooldownReduction(bonus);
    }

    public override void OnDelete()
    {
        PlayerStatsSO.Instance.ChangeCooldownReduction(bonus * -1);
    }

}
