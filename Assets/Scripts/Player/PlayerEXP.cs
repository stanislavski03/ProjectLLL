using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerEXP : MonoBehaviour
{
    [SerializeField] private float _maxEXP;

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
        EXPChanged?.Invoke(_currentEXP);
    }

    private void LevelUP()
    {
        _currentLVL += 1;
    }
}
