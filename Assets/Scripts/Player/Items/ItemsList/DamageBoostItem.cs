using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class DamageBoostItem : MonoBehaviour
{
    public ItemDataSO data;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            data.OnPick();
            Destroy(gameObject);
        }
    }
}
