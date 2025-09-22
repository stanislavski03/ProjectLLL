using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class LvlUpItemsInfo : MonoBehaviour
{
    public List<GameObject> ItemsList;
    public List<Weapon> weaponList;



    public void SetItemsInfo()
{
    for (int i = 0; i < Mathf.Min(ItemsList.Count, weaponList.Count); i++)
    {
        // Пропускаем null элементы
        if (ItemsList[i] == null || weaponList[i] == null) continue;
        
        // Ищем TextMeshProUGUI напрямую среди дочерних объектов
        TextMeshProUGUI TMPItemTitle = ItemsList[i].GetComponentInChildren<TextMeshProUGUI>(true);

            if (TMPItemTitle != null)
            {
                TMPItemTitle.text = weaponList[i].GetTextTitleInfo();
            }
    }
}
}