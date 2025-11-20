using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightInitialisation : MonoBehaviour
{
    [SerializeField] private Vector3 DirectionNeeded;
    void Start()
    {
        Vector3 direction = DirectionNeeded - transform.position;
        transform.rotation = Quaternion.Euler(direction);
    }


}
