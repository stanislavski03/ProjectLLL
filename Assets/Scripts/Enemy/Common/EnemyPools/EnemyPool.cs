using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPool : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    private Queue<GameObject> enemyPool = new Queue<GameObject>();


    public void EnquePool()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }
        enemyPool.Clear();
    }
    public void GetEnemy(Vector3 position)
    {
        if (Generation.Instance.CheckPointForLegitment(position))
        {
            if (NavMesh.SamplePosition(position, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                GameObject enemy;
                if (enemyPool.Count > 0)
                {
                    enemy = enemyPool.Dequeue();
                    enemy.SetActive(true);
                    enemy.transform.position = new Vector3(hit.position.x, enemyPrefab.transform.position.y, hit.position.z);
                }
                else
                {
                    enemy = Instantiate(enemyPrefab, position, Quaternion.identity);
                    enemy.transform.SetParent(transform, true);
                    enemy.transform.position = new Vector3(hit.position.x, enemyPrefab.transform.position.y, hit.position.z);
                    enemy.GetComponent<EnemyHP>()._pool = gameObject.GetComponent<EnemyPool>();
                }
                enemy.transform.LookAt(new Vector3(Player.Instance.transform.position.x, enemy.transform.position.y, Player.Instance.transform.position.z));
            }
        }
    }

    public void GetEnemyBackToPool(GameObject enemy)
    {
        enemy.SetActive(false);
        enemyPool.Enqueue(enemy);
    }
}
