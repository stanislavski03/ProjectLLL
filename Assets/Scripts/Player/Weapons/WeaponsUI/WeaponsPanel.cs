using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponsPanel : MonoBehaviour
{
    public static WeaponsPanel Instance { get; private set; }
    public LvlUpWeaponItemsInfo LvlUpWeaponItemsInfo;
    public List<WeaponDataSO> AllPlayerWeapons;
    public List<WeaponDataSO> currentPlayerWeapons;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            currentPlayerWeapons = WeaponController.Instance.currentPlayerWeapons;
        }
    }

    public void SetWeaponsInfo()
    {
        Transform[] items = new Transform[transform.childCount];

        int indexOfCurrentItemForSetACtive = 0;


        for (int i = 0; i < transform.childCount; i++)
        {
            items[i] = transform.GetChild(i);
        }

        for (int i = 0; i < currentPlayerWeapons.Count; i++)
        {

            if (currentPlayerWeapons[i] != null)
            {
                items[i].GetComponent<UnityEngine.UI.Image>().sprite = currentPlayerWeapons[i].icon;
                items[i].GetComponentInChildren<TextMeshProUGUI>(false).text = currentPlayerWeapons[i].currentWeapon.currentLevel.ToString();
            }
        }

        for (int i = 0; i < items.Length; i++)
        {
            if (i >= currentPlayerWeapons.Count)
            {
                return;
            }
            if (currentPlayerWeapons[i] != null)
            {
                items[indexOfCurrentItemForSetACtive].gameObject.SetActive(true);
                indexOfCurrentItemForSetACtive++;
            }
        }
    }

    public void ClearWeaponsList()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }


}
