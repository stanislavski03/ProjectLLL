using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    [SerializeField] private float _EXPamount;
    [SerializeField] private long _moneyAmount;
    private bool _gathered = false;



    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ExpGatherTrigger>())
        {
            _gathered = true;
            StartCoroutine(ApproachToPlayer());
        }

        if (other.GetComponent<Player>())
        {
            Player.Instance.GetComponent<PlayerEXP>().GetEXP(_EXPamount);
            PlayerStatsSO.Instance.ChangeMoney(_moneyAmount);
            Destroy(gameObject);
        }
    }
     private IEnumerator ApproachToPlayer()
    {
        while (_gathered)
        {
            if (Player.Instance != null)
            {
                transform.position += (Player.Instance.transform.position - transform.position).normalized * 15 * Time.deltaTime;
                yield return new WaitForFixedUpdate();
            }
            else
                yield return null;
        }
    }
}
