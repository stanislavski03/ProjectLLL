using UnityEngine;
[CreateAssetMenu(fileName = "New Mutation Max HP Data", menuName = "Mutations/Mutation Max HP Data")]
public class MutationMaxHPSO : MutationDataSO
{
    public override void OnPick()
    {
        PlayerStatsSO.Instance.ChangeMaxHp(bonus);
    }

    public override void OnDelete()
    {
        PlayerStatsSO.Instance.ChangeMaxHp(bonus * -1);
    }

}
