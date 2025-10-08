using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryInfo : MonoBehaviour
{
    public List<Weapon> allWeaponList;

    public void SetInventoryItemsInfo()
    {
        Transform[] inventoryItems = new Transform[transform.childCount];

        int indexOfCurrentItemForSetInfo = 0;
        int indexOfCurrentItemForSetACtive = 0;


        for (int i = 0; i < transform.childCount; i++)
        {
            inventoryItems[i] = transform.GetChild(i);
        }

        for (int i = 0; i < allWeaponList.Count; i++)
        {

            if (allWeaponList[i] != null && allWeaponList[i].currentLevel != 0)
            {
                TextMeshProUGUI[] inventoryItemTexts = inventoryItems[indexOfCurrentItemForSetInfo].GetComponentsInChildren<TextMeshProUGUI>();
                if (inventoryItemTexts.Length >= 3)
                {
                    inventoryItemTexts[0].SetText(allWeaponList[i].GetTextTitleInfo());
                    inventoryItemTexts[1].SetText(allWeaponList[i].GetTextDescriptionInfo());
                    inventoryItemTexts[2].SetText(allWeaponList[i].GetWeaponStats());
                }
                indexOfCurrentItemForSetInfo++;
            }
        }

        for (int i = 0; i < inventoryItems.Length; i++)
        {

            if (i >= allWeaponList.Count)
                return;
            if (allWeaponList[i] != null && allWeaponList[i].currentLevel != 0)
            {
                inventoryItems[indexOfCurrentItemForSetACtive].gameObject.SetActive(true);
                indexOfCurrentItemForSetACtive++;
            }
        }
    }

    public void ClearWeaponAndItemList()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}