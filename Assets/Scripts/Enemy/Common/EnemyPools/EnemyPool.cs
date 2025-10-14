using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();



    public void GetEnemy(Vector3 position)
    {

        if (enemyPool.Count > 0)
        {
            GameObject enemy = enemyPool.Dequeue();
            enemy.SetActive(true);
            enemy.transform.position = new Vector3(position.x, 0.5f, position.z);
        }
        else
        {
            GameObject enemy = Instantiate(enemyPrefab, transform);
            enemy.transform.position = new Vector3(position.x, 0.5f, position.z);
            enemy.GetComponent<EnemyHP>()._pool = gameObject.GetComponent<EnemyPool>();
        }
    }

    public void GetEnemyBackToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
