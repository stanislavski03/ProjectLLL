using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "New TechInstrumentsSO Data", menuName = "Items/TechInstrumentsSO Data")]
public class TechInstrumentsSO : ItemDataSO
{
    [SerializeField] private int _levelUpsAmount;

    public override void OnPick()
    {
        base.OnPick();
        GiveLvl();
    }

    private async void GiveLvl()
    {
        await UniTask.WaitForSeconds(1);
        PlayerEXP.Instance.HandleMultipleLevelUpsWithNoCheck(_levelUpsAmount).Forget();
    }

}
