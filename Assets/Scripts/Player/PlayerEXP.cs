using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerEXP : MonoBehaviour
{
    [SerializeField] private float _maxEXP;
    [SerializeField] private Image _expProgressBarImage;

    public event Action<float> EXPChanged;
    public event Action<float> LVLChanged;

    private float _currentEXP;
    private float _currentLVL;

    public float MaxEXP => _maxEXP;

    private void OnEnable()
    {
        _currentEXP = 0;
        _currentLVL = 1;
    }

    public void GetEXP(float EXPamount)
    {
        _currentEXP += EXPamount;
        if (_currentEXP >= _maxEXP)
        {
            _currentEXP = _currentEXP - _maxEXP;
            LevelUP();
            LVLChanged?.Invoke(_currentLVL);
        }

        _expProgressBarImage.fillAmount = Mathf.Clamp01(_currentEXP/MaxEXP);

        EXPChanged?.Invoke(_currentEXP);
    }

    private void LevelUP()
    {
        _currentLVL += 1;
    }
}
