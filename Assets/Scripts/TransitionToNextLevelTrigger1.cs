using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransitionToNextLevelTrigger1 : MonoBehaviour
{
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            TransitionManager.Instance.TransitPlayerToLevel(1);
        }
    }
}
