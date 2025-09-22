using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LvlUpItemsInfo : MonoBehaviour
{


    private List<Weapon> _currentWeaponList = new List<Weapon>();
    private List<Weapon> weaponList = new List<Weapon>();

    public List<GameObject> ItemsList;
    public List<Weapon> allWeaponList;
    public int numberOfCurrentWeaponList = 2;

    private void Start()
    {
        SetWeaponList();
    }

    public void SetWeaponList()
    {
        for (int i = 0; i < allWeaponList.Count; i++)
        {
            if (allWeaponList[i].isActiveAndEnabled)
            {
                weaponList.Add(allWeaponList[i]);
            }
        }
    }

    public void TransferRandomObjects()
    {
        int countToTransfer = Mathf.Min(numberOfCurrentWeaponList, weaponList.Count);

        for (int i = 0; i < countToTransfer; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, weaponList.Count);
            Weapon randomObject = weaponList[randomIndex];

            _currentWeaponList.Add(randomObject);

            weaponList.RemoveAt(randomIndex);
        }
    }

    public void SetItemsInfo()
    {
        TransferRandomObjects();

        for (int i = 0; i < Mathf.Min(ItemsList.Count, _currentWeaponList.Count); i++)
        {
            // Пропускаем null элементы
            if (ItemsList[i] == null || _currentWeaponList[i] == null) continue;

            // Ищем TextMeshProUGUI напрямую среди дочерних объектов
            TextMeshProUGUI[] TMPItemTitle = ItemsList[i].GetComponentsInChildren<TextMeshProUGUI>(true);

            if (TMPItemTitle != null)
            {
                TMPItemTitle[0].text = _currentWeaponList[i].GetTextTitleInfo();
                TMPItemTitle[1].text = _currentWeaponList[i].GetTextDescriptionInfo();
            }
        }
    }
}