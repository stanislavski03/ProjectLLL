using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerReputationEditTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerStatsSO.Instance.ChangeReputation(10);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            PlayerStatsSO.Instance.ChangeReputation(-10);
            
        }
    }
}
