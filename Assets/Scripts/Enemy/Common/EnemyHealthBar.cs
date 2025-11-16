using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider _healhtBar;
    [SerializeField] private EnemyHP _enemyHP;

    private void OnEnable()
    {
        //_enemyHP.onDamage += GetDamage;

        _healhtBar.maxValue = _enemyHP.GetMaxHP();
        _healhtBar.minValue = 0;
        _healhtBar.value = _enemyHP.GetMaxHP();
    }

    private void GetDamage(float damageAmmount)
    {
        _healhtBar.value = _enemyHP.GetHP();
    }
}
