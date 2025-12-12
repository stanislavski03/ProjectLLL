using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TilePointsTrigger : MonoBehaviour
{
    private bool Visited = false;
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Player>() && !Visited)
        {
            Visited = true;
            EnemySpawnManager.Instance.GainDifficulty(0.1f);
        }
    }

}
