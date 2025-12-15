using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReposition : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Enemy>())
        {
            other.transform.position = EnemySpawner.Instance.GetRandomEdgePoint();
        }
    }
    private void Update()
    {
        transform.position = new Vector3(Player.Instance.transform.position.x + 4,0, Player.Instance.transform.position.z + 50);
        transform.rotation = Quaternion.identity;
    }
}
