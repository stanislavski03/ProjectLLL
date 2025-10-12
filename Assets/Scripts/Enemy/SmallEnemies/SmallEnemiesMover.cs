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

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        FindPlayer();
        InitializeAgent();
    }

    private void FindPlayer()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerTransform = player.transform;
        }
        else
        {
            enabled = false;
            return;
        }
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
                    Debug.LogWarning("NavMeshAgent cannot be placed on NavMesh.");
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
    }

    private void FixedUpdate()
    {
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