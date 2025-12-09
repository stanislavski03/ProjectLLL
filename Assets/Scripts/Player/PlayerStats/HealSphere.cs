using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSphere : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            PlayerHP.Instance.Heal(50);
            Destroy(gameObject);
        }
    }
}
