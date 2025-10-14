using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemyMeleePool : EnemyPool
{
    public static SmallEnemyMeleePool Instance;
    private void Awake()
    {
        Instance = this;
    }
}
