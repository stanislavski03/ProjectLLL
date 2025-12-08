using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MinotourBossUnickAttacks : MonoBehaviour
{
    private float cooldownTimer = 5;
    private float cooldown = 0;
    private bool isAttacking = false;
    NavMeshAgent NavMesh;
    EnemiesMover Mover;
    private void Start()
    {
        NavMesh = GetComponent<NavMeshAgent>();
        Mover = GetComponent<EnemiesMover>();

    }

    private void Update()
    {
        if (cooldown >= cooldownTimer & !isAttacking)
        {
            isAttacking = true;
            Attack();
        }
        cooldown += Time.fixedDeltaTime;

    }
    private void Attack()
    {
        SpawnAttack();
    }


    private async void SpawnAttack()
    {
        Debug.Log("SpawnAttack");
        NavMesh.isStopped = true;
        await UniTask.WaitForSeconds(1);
        for (int i = 0; i<10; i++) {
            EnemySpawner.Instance.SpawnFastMeleeEnemy();
            EnemySpawner.Instance.SpawnSmallMeleeEnemy();
            EnemySpawner.Instance.SpawnSmallRangedEnemy();
            EnemySpawner.Instance.SpawnStrongMeleeEnemy();
        }
        NavMesh.isStopped = false;
        isAttacking = false;
        cooldown = 0;
    }

    private void SlamAttack()
    {

    }

    private void RushAttack()
    {

    }

}
