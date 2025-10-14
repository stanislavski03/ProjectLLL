using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolList : MonoBehaviour
{
    public static EnemyPoolList instance;
    public EnemyPool _smallMeleeEnemy;
    public EnemyPool _smallRangedEnemy;
    private void Awake()
    {
        instance = this;
    }
}
