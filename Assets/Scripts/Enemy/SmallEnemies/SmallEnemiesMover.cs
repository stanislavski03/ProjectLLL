using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemiesMover : MonoBehaviour
{
    [SerializeField] private Transform goal;

    private NavMeshAgent agent;
    [SerializeField] private EnemyConfig _initializedStats;

    private void OnEnable()
    {
        try
        {
            agent.speed = _initializedStats._moveSpeed;
        }
        catch { }
    }

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    private void FixedUpdate()
    {
        agent.destination = goal.position;
    }

}
