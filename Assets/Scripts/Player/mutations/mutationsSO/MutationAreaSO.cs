using UnityEngine;
[CreateAssetMenu(fileName = "New Mutation Area Data", menuName = "Mutations/Mutation Area Data")]
public class MutationAreaSO : MutationDataSO
{
    public override void OnPick()
    {
        PlayerStatsSO.Instance.ChangeAreaMultiplier(bonus);
    }

    public override void OnDelete()
    {
        PlayerStatsSO.Instance.ChangeAreaMultiplier(bonus * -1);
    }

}
