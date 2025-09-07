using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageField : MonoBehaviour
{

    [SerializeField] private float _damagePeriod = 0.2f;
    [SerializeField] private float _damageAmmount = 10;

    private List<EnemyHP> targets = new List<EnemyHP>();

    private void OnEnable()
    {
        StartCoroutine(Damager());
    }
    private void OnDisable()
    {
        StopCoroutine(Damager());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHP>())
        {
            targets.Add(other.GetComponent<EnemyHP>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<EnemyHP>())
        {
            targets.Remove(other.GetComponent<EnemyHP>()); 
        }
    }

    IEnumerator Damager()
    {
        while (true)
        {
            if (targets.Count == 0)
            {
                enabled = false;
                break;
            }
            for (int i = 0; i < targets.Count; i++)
            {
                
                targets[i].Damage(_damageAmmount);
                if (targets[i].GetHP() <= 0)
                {
                    targets.Remove(targets[i]);
                }
            }
            yield return new WaitForSeconds(_damagePeriod);
        }
    }

}
