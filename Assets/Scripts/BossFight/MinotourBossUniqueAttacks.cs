using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class MinotourBossUnickAttacks : MonoBehaviour
{
    [SerializeField] private GameObject _explosionPrefab;

    public AudioSource _audioSource;
    public AudioSource _audioSource2;
    public AudioClip _spawnAttackSound;
    public AudioClip _slamAttackSound;
    public AudioClip _rushAttackSound;
    public AudioClip _prepareRushAttackSound;

    [SerializeField] private float cooldownTimer = 4;
    private float cooldown = 0;
    private bool isAttacking = false;
    NavMeshAgent NavMesh;
    EnemiesMover Mover;

    [SerializeField] private Animator animator;
    private void Start()
    {
        NavMesh = GetComponent<NavMeshAgent>();
        Mover = GetComponent<EnemiesMover>();

    }

    private void Update()
    {
        if (cooldown >= cooldownTimer && !isAttacking)
        {
            isAttacking = true;
            Attack();
        }
        cooldown += Time.deltaTime;

    }
    private void Attack()
    {
        int randAttack = Random.Range(0, 3);
        switch (randAttack)
        {
            case 0:
                SpawnAttack();
                break;
            case 1:
                SlamAttack();
                break;
            case 2:
                RushAttack();
                break;
            default:
                SpawnAttack();
                break;
        }

    }


    private async void SpawnAttack()
    {
        Debug.Log("SpawnAttack");
        NavMesh.isStopped = true;
        _audioSource.PlayOneShot(_spawnAttackSound);
        animator.SetBool("SpawnAttack", true);
        await UniTask.WaitForSeconds(1);
        for (int i = 0; i < 10; i++)
        {
            EnemySpawner.Instance.SpawnFastMeleeEnemy();
            await UniTask.WaitForSeconds(0.02f);
            EnemySpawner.Instance.SpawnSmallMeleeEnemy();
            await UniTask.WaitForSeconds(0.02f);
            EnemySpawner.Instance.SpawnSmallRangedEnemy();
            await UniTask.WaitForSeconds(0.02f);
            EnemySpawner.Instance.SpawnStrongMeleeEnemy();
            await UniTask.WaitForSeconds(0.02f);
        }
        NavMesh.isStopped = false;
        isAttacking = false;
        cooldown = 0;
        animator.SetBool("SpawnAttack", false);
    }

    private async void SlamAttack()
    {
        Debug.Log("SlamAttack");
        NavMesh.isStopped = true;
        _audioSource.PlayOneShot(_slamAttackSound);
        animator.SetBool("SlamAttack", true);
        await UniTask.WaitForSeconds(1);

        cooldown = 0;

        for (int i = 0; i < 10; i++)
        {
            GameObject SpawnedExplosion = Instantiate(_explosionPrefab, GetPositionAroundPlayer(), Quaternion.identity);
            SpawnedExplosion.SetActive(true);
        }
        NavMesh.isStopped = false;
        isAttacking = false;
        animator.SetBool("SlamAttack", false);



    }

    private Vector3 GetPositionAroundPlayer()
    {
        Vector3 Rand = new Vector3(Player.Instance.transform.position.x + Random.Range(-30f, 30f), 0, Player.Instance.transform.position.z + Random.Range(-30f, 30f));
        return Rand;
    }

    private async void RushAttack()
    {
        Debug.Log("RushAttack");
        NavMesh.isStopped = true;
        
        for (int i = 3; i > 0; i--)
        {
            animator.SetBool("RushAttack", false);
            animator.SetBool("PreparingRushAttack", true);
             _audioSource2.PlayOneShot(_prepareRushAttackSound);
            await UniTask.WaitForSeconds(1);
            animator.SetBool("PreparingRushAttack", false);
            animator.SetBool("RushAttack", true);
             _audioSource.PlayOneShot(_rushAttackSound);
            
            Vector3 OldRunTowardsPosition;
            float timer = 1.5f;
            Vector3 PlayerPositionBeforeCheck = Player.Instance.transform.position;
            OldRunTowardsPosition = PlayerPositionBeforeCheck - transform.position;
            Vector3 NewRunTowardsPosition;
            transform.rotation = Quaternion.LookRotation(OldRunTowardsPosition.normalized);

            while (timer >= 0)
            {
                if (OldRunTowardsPosition != (Player.Instance.transform.position - transform.position))
                {

                    //Vector3 PositionDifference = Player.Instance.transform.position - PlayerPositionBeforeCheck;

                    OldRunTowardsPosition = PlayerPositionBeforeCheck - transform.position;
                    //PositionDifference = PositionDifference.normalized * 0.001f;
                    NewRunTowardsPosition = Player.Instance.transform.position - transform.position;

                    float AngleDifference = Vector3.SignedAngle(OldRunTowardsPosition, NewRunTowardsPosition, Vector3.up);


                    if (AngleDifference > 0.7f)
                        AngleDifference = 0.7f;
                    else if (AngleDifference < -0.7f)
                        AngleDifference = -0.7f;
                    transform.Rotate(Vector3.up, AngleDifference);
                    //= Quaternion.AngleAxis(Vector3.AngleDifference(NewRunTowardsPosition, OldRunTowardsPosition) + transform.rotation.y, Vector3.up);


                }


                transform.position += Time.fixedDeltaTime * transform.forward * 30;

                timer -= Time.fixedDeltaTime;
                PlayerPositionBeforeCheck = Player.Instance.transform.position;

                await UniTask.WaitForFixedUpdate();

            }
        }

        NavMesh.isStopped = false;
        isAttacking = false;
        cooldown = 0;
        animator.SetBool("RushAttack", false);

    }

}
