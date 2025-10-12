using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemiesMover : MonoBehaviour
{
    [SerializeField] private Transform goal;
    RaycastHit hit;
    private float _range = 20f;
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
        
        if (Vector3.Distance(transform.position, goal.position) <= _range)
        {
            if(Physics.Raycast(transform.position, goal.position, out hit, _range, LayerMask.GetMask("Player") | LayerMask.GetMask("Wall")))
            {
                if (hit.collider.tag == "Player")
                {
                    agent.speed = 0;
                }
                else agent.speed = _initializedStats._moveSpeed;
            }
            else agent.speed = _initializedStats._moveSpeed;

        }
        else agent.speed = _initializedStats._moveSpeed;


        agent.destination = goal.position;
    }

}
