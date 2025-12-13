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
        Debug.Log("timer start");
        await UniTask.WaitForSeconds(5);
        Debug.Log("timer end");
        Player.Instance.DamageGiveShield.SetActive(false);
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
        PlayerStatsSO.Instance.invincibility = false;
        
        StartInvincibilityTimerDelay();
    }

    private async void StartInvincibilityTimerDelay()
    {
        OnDelay = true;
        Debug.Log("OnDelay = true");
        await UniTask.WaitForSeconds(5);
        Debug.Log("OnDelay = false");
        OnDelay = false;
    }
}
