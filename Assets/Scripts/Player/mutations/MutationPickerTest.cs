using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationPickerTest : MonoBehaviour
{
    public MutationDataSO mutation;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            MutationControllerSO.Instance.AddMutation(mutation);
            Destroy(gameObject);
        }
    }
}
