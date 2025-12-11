using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class PlayerReputationView : MonoBehaviour
{
    [SerializeField] private Image _magicProgressBarImage;
    [SerializeField] private Image _technoProgressBarImage;


    void Start()
    {
        UpdateProgressBar(PlayerStatsSO.Instance.Reputation);
        
    }

    private void OnEnable()
    {
        PlayerStatsSO.Instance._reputationChanged += UpdateProgressBar;
    }
    
    private void OnDisable()
    {
        PlayerStatsSO.Instance._reputationChanged -= UpdateProgressBar;
    }

    private void UpdateProgressBar(float Reputation)
    {
        if(Reputation == 50)
        {
            _magicProgressBarImage.fillAmount = 0;
            _technoProgressBarImage.fillAmount = 0;
        }
        else if(Reputation > 50)
        {
            _magicProgressBarImage.fillAmount = 0;
            _technoProgressBarImage.fillAmount = Mathf.Clamp01((Reputation - 50) / 50);
        }
        else if(Reputation < 50)
        {
            _technoProgressBarImage.fillAmount = 0;
            _magicProgressBarImage.fillAmount = Mathf.Clamp01((Reputation - 50) / -50);
        }
    }
 }
