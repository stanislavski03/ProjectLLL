using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageFieldTrigger : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<EnemyHP>()) gameObject.GetComponent<DamageField>().enabled = true;

    }

}
