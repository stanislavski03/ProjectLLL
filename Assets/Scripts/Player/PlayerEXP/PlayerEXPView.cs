using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerEXPView : MonoBehaviour
{
    [SerializeField] private PlayerEXP _playerEXP;
    [SerializeField] private TextMeshProUGUI _expText;
    [SerializeField] private TextMeshProUGUI _lvlText;

    void Start()
    {
        _expText.text = "0 / " + _playerEXP.MaxEXP.ToString("0");
        _lvlText.text = "1";
    }

    private void OnEnable()
    {
        _playerEXP.EXPChanged += EXPupdate;
        _playerEXP.LVLChanged += LVLupdate;
    }
    private void OnDisable()
    {
        _playerEXP.EXPChanged -= EXPupdate;
        _playerEXP.LVLChanged -= LVLupdate;
    }

    private void EXPupdate(float currentEXP)
    {
        _expText.text = currentEXP.ToString("0") + " / " + _playerEXP.MaxEXP.ToString("0");

    }
    
    private void LVLupdate(float currentLVL)
    { 
        _lvlText.text = currentLVL.ToString("0");
    
    }
}
