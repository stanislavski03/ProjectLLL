using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHPView : MonoBehaviour
{


    [SerializeField] private PlayerHP _health;
    [SerializeField] private TextMeshProUGUI _healthText;

    // Start is called before the first frame update
    void Start()
    {
        _healthText.text = _health.MaxHP.ToString("0");
    }
    private void OnEnable()
    {
        _health.Changed += TakeDamage;
    }
    private void OnDisable()
    {
        _health.Changed -= TakeDamage;
    }

    private void TakeDamage(float currentHP)
    { 
        _healthText.text = currentHP.ToString("0");
    
    }
 }
