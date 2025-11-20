using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;

public class ClearAllPools : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ItemControllerSO.Instance.ClearAllPools();
            WeaponController.Instance.ClearPool();
        }
    }
}