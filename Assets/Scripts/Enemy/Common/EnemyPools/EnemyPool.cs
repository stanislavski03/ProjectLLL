using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();



    public void GetEnemy(Vector3 position)
    {
        if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
        {
            if (enemyPool.Count > 0)
            {
                GameObject enemy = enemyPool.Dequeue();
                enemy.SetActive(true);
                enemy.transform.position = new Vector3(hit.position.x, 0.5f, hit.position.z);
            }
            else
            {
                GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                enemy.transform.SetParent(transform, true); 
                enemy.transform.position = new Vector3(hit.position.x, 0.5f, hit.position.z);
                enemy.GetComponent<EnemyHP>()._pool = gameObject.GetComponent<EnemyPool>();
            }
        }
        else
        {
            Debug.LogWarning($"Cannot spawn enemy at {position} - no NavMesh nearby");
        }

    }

    public void GetEnemyBackToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
