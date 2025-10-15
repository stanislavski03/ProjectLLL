using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolList : MonoBehaviour
{
    public static EnemyPoolList instance;
    public EnemyPool _smallMeleeEnemy;
    public EnemyPool _smallRangedEnemy;
    public EnemyPool _fastMeleeEnemy;
    public EnemyPool _strongMeleeEnemy;
    private void Awake()
    {
        instance = this;
    }
}
