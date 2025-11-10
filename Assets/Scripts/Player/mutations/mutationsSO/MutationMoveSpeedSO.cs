using UnityEngine;
[CreateAssetMenu(fileName = "New Mutation Move Speed Data", menuName = "Mutations/Mutation Move Speed Data")]
public class MutationMoveSpeedSO : MutationDataSO
{
    public override void OnPick()
    {
        PlayerStatsSO.Instance.ChangeMoveSpeed(bonus);
    }

    public override void OnDelete()
    {
        PlayerStatsSO.Instance.ChangeMoveSpeed(bonus * -1);
    }

}
