using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform Player;

    void Update()
    {
        transform.LookAt(Player);
    }
}
