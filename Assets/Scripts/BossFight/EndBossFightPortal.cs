using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndBossFightPortal : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Player.Instance.TriggerWin();
        }
    }
}
