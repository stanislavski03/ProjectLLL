using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class BossHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private EnemyHP _health;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private Image _healthProgressBarImage;

    public GameObject _portal;


    void Start()
    {
        _name.text = _health._initializedStats._enemyName;
        _healthText.text = _health.GetMaxHP().ToString("0") + " / " + _health.GetMaxHP().ToString("0");
    }

    private void OnEnable()
    {
        _health.EnemyHPCnanged += TakeDamage;
;
    }
    
    private void OnDisable()
    {
        _health.EnemyHPCnanged -= TakeDamage;
    }

    private void TakeDamage()
    {
        _healthText.text = _health.GetHP().ToString("0") + " / " + _health.GetMaxHP().ToString("0");

        if (_healthProgressBarImage != null)
        {
            _healthProgressBarImage.fillAmount = Mathf.Clamp01(_health.GetHP() / _health.GetMaxHP());
        }

        if(_health.GetHP() <= 0)
        {
            EndBossFight();
        }
    }

    private async void EndBossFight()
    {
        await UniTask.WaitForSeconds(5);
        Instantiate(_portal, new Vector3(50, 1, 50), Quaternion.identity);
    }
 }
