using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitializationManager : MonoBehaviour
{

    void Start()
    {
        ItemControllerSO.Instance.ClearAllPools();
        WeaponController.Instance.ClearPool();
        MutationControllerSO.Instance.ClearAllMutations();

        
        ResetPlayerStats();
    }

    public void ResetPlayerStats()
    {
        PlayerStatsSO stats = PlayerStatsSO.Instance;
        stats.DamageMultiplier = 1;
        stats.MagicDamageMultiplier = 1;
        stats.TechnoDamageMultiplier = 1;
        stats.CooldownReduction = 1;
        stats.AreaMultiplier = 1;
        stats.MaxHP = 100;
        stats.MoveSpeed = 15;
        stats.SpeedMultiplier = 1;
        stats.Money = 100;
        stats.Reputation = 50;
        stats.maxEXP = 100;
        stats.maxLevel = 30;
        stats.invincibility = false;
    }

}
