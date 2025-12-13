using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements.Experimental;

public class EnemyPoolList : MonoBehaviour
{
    public static EnemyPoolList instance;
    public EnemyPool _smallMeleeEnemy;
    public EnemyPool _smallRangedEnemy;
    public EnemyPool _fastMeleeEnemy;
    public EnemyPool _strongMeleeEnemy;

    public void EnquePools()
    {
        _smallMeleeEnemy.EnquePool();
        _smallRangedEnemy.EnquePool();
        _fastMeleeEnemy.EnquePool();
        _strongMeleeEnemy.EnquePool();
    }

    private void Awake()
    {
        instance = this;
    }
}
