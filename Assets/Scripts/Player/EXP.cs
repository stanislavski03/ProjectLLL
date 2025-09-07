using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EXP : MonoBehaviour
{
    [SerializeField] private float _EXPamount;

    private PlayerEXP _playerEXP;

    private void Awake()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            _playerEXP = player.GetComponent<PlayerEXP>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _playerEXP.GetEXP(_EXPamount);
            Destroy(gameObject);
        }
    }
}
