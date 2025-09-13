using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SmallEnemiesMover : MonoBehaviour
{

    [SerializeField] private Transform goal;
    private NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }
    private void Update()
    {
        agent.destination = goal.position;
    }

}
