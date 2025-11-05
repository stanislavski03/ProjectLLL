using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MutationDeleteTest : MonoBehaviour
{
    public MutationDataSO mutation;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            if (MutationControllerSO.Instance.PlayerMutationsPool.Contains(mutation))
            {
                
                MutationControllerSO.Instance.DeleteMutation(mutation);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("Нет мутации для удаления");
            }

        }
    }
}
