using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    [SerializeField] private float _EXPamount;
    [SerializeField] private long _moneyAmount;




    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            Player.Instance.GetComponent<PlayerEXP>().GetEXP(_EXPamount);
            PlayerStatsSO.Instance.ChangeMoney(_moneyAmount);
            Destroy(gameObject);
        }
    }
}
