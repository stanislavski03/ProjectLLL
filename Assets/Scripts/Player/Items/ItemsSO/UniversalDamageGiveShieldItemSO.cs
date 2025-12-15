using UnityEngine;
using Cysharp.Threading.Tasks;

[CreateAssetMenu(fileName = "New UniversalDamageGiveShieldItemSO Data", menuName = "Items/UniversalDamageGiveShieldItemSO Data")]
public class UniversalDamageGiveShieldItemSO : ItemDataSO
{
    public bool OnDelay = false;

    public override void OnPick()
    {
        base.OnPick();
    }

    public override void OnDamageGive()
    {

        if(OnDelay == false)
            StartInvincibilityTimer();
    }

    private async void StartInvincibilityTimer()
    {
        PlayerStatsSO.Instance.invincibility = true;
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Player.Instance.DamageGiveShield.SetActive(true);
        await UniTask.WaitForSeconds(5);
        Player.Instance.DamageGiveShield.SetActive(false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        PlayerStatsSO.Instance.invincibility = false;
        
        StartInvincibilityTimerDelay();
    }

    private async void StartInvincibilityTimerDelay()
    {
        OnDelay = true;
        await UniTask.WaitForSeconds(5);
        OnDelay = false;
    }
}
