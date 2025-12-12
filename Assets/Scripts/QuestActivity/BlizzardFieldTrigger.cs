using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class BlizzardFieldTrigger : MonoBehaviour
{
    private bool _isPlayerSafe = false;
    public float damagePeriod;
    public int damage;


    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _isPlayerSafe = true;
            StartDoingProgress();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _isPlayerSafe = false;
            StartDamagePlayerForLeaving();
        }
    }

    private async void StartDoingProgress()
    {
        while (_isPlayerSafe)
        {
            QuestManager.Instance.BlizzardShieldProgressForOne();
            await UniTask.WaitForSeconds(1);
        }
    }

    private async void StartDamagePlayerForLeaving()
    {
        while (!_isPlayerSafe)
        {
            PlayerHP.Instance.Damage(damage);
            await UniTask.WaitForSeconds(damagePeriod);

        }
    }
}
