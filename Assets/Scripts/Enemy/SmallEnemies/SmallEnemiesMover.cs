using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemiesMover : MonoBehaviour
{
    private Transform _playerTransform;
    private NavMeshAgent agent;
    [SerializeField] private EnemyConfig _initializedStats;
    private bool isReady = false;
    private float _moveSpeedDeviation;
    private Rigidbody rb;
    [SerializeField] private bool shouldReposition = true;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        InitializeAgent();
        _playerTransform = Player.Instance.transform;
    }


    private void InitializeAgent()
    {
        if (agent != null && _initializedStats != null)
        {
            agent.speed = _initializedStats._moveSpeed + Random.Range(-_moveSpeedDeviation, _moveSpeedDeviation);

            if (!agent.isOnNavMesh)
            {
                if (!agent.Warp(transform.position))
                {
                    return;
                }
            }

            isReady = true;
        }
    }

    private void OnEnable()
    {
        _moveSpeedDeviation = _initializedStats._moveSpeedDeviation;
        if (agent != null)
        {
            agent.speed = _initializedStats._moveSpeed;
        }
        transform.localScale = Vector3.one * _initializedStats._size;

        rb = GetComponent<Rigidbody>();

        rb.maxLinearVelocity = 0;
    }

    private void Update()
    {
        if (shouldReposition)
        {

            if (transform.position.z > Player.Instance.transform.position.z + 30)
            {
                Vector3 position = new Vector3(transform.position.x - 2 * (transform.position.x - Player.Instance.transform.position.x), transform.position.y, Player.Instance.transform.position.z - 15);
                if (Generation.Instance.CheckPointForLegitment(position)) 
                    transform.position = position;
            }
            else if (transform.position.z < Player.Instance.transform.position.z - 15)
            {
                Vector3 position = new Vector3(transform.position.x - 2 * (transform.position.x - Player.Instance.transform.position.x), transform.position.y, Player.Instance.transform.position.z + 30);
                if (Generation.Instance.CheckPointForLegitment(position))
                    transform.position = position;

            }
            else if (transform.position.x > Player.Instance.transform.position.x + 30)
            {
                Vector3 position = new Vector3(Player.Instance.transform.position.x - 30, transform.position.y, transform.position.z - 2 * (transform.position.z - Player.Instance.transform.position.z));
                if (Generation.Instance.CheckPointForLegitment(position))
                    transform.position = position;
            }
            else if (transform.position.x < Player.Instance.transform.position.x - 30)
            {
                Vector3 position = new Vector3(Player.Instance.transform.position.x + 30, transform.position.y, transform.position.z - 2 * (transform.position.z - Player.Instance.transform.position.z));
                if (Generation.Instance.CheckPointForLegitment(position))
                    transform.position = position;
            }
        }



        if (!isReady || !CanSetDestination()) return;

        agent.SetDestination(_playerTransform.position);
    }

    private bool CanSetDestination()
    {
        return agent != null &&
               agent.isActiveAndEnabled &&
               agent.isOnNavMesh &&
               _playerTransform != null;
    }
}