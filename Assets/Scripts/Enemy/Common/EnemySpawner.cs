using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    private void Start()
    {
        GameObject enemyObj = EnemyPool.Instance.GetEnemy();
        enemyObj.SetActive(true);
        enemyObj.transform.position = new Vector3(0,0.5f,0);
    }
}
