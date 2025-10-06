using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpItemsInfo : MonoBehaviour
{
    private List<Weapon> _currentWeaponList = new List<Weapon>();
    private List<Weapon> weaponList = new List<Weapon>();

    public List<GameObject> ItemsList;
    public List<Weapon> allWeaponList;
    public int numberOfCurrentWeaponList = 2;

    public LevelUpController levelUpController;

    private void Start()
    {
        SubscribeToAllWeapons();
        SetWeaponList();
    }

    private void OnEnable()
    {
        SubscribeToAllWeapons();
        SetWeaponList();
    }

    private void OnDisable()
    {
        UnsubscribeFromAllWeapons();
    }

    private void SubscribeToAllWeapons()
    {
        foreach (var weapon in allWeaponList)
        {
            if (weapon != null)
            {
                weapon.AvailableChanged -= OnWeaponAvailableChanged;
                weapon.AvailableChanged += OnWeaponAvailableChanged;
            }
        }
    }

    private void UnsubscribeFromAllWeapons()
    {
        foreach (var weapon in allWeaponList)
        {
            if (weapon != null)
            {
                weapon.AvailableChanged -= OnWeaponAvailableChanged;
            }
        }
    }

    private void OnWeaponAvailableChanged()
    {
        SetWeaponList();
    }

    public void SetWeaponList()
    {
        weaponList.Clear();
        
        for (int i = 0; i < allWeaponList.Count; i++)
        {
            if (allWeaponList[i] != null && allWeaponList[i].IsAvailable)
            {
                weaponList.Add(allWeaponList[i]);
            }
        }
        

    }

    public void TransferRandomObjects()
    {
        _currentWeaponList.Clear();
        
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
        SetWeaponList();
        TransferRandomObjects();

        foreach (var item in ItemsList)
        {
            if (item != null) 
                item.SetActive(false);
        }

        for (int i = 0; i < Mathf.Min(ItemsList.Count, _currentWeaponList.Count); i++)
        {
            if (ItemsList[i] == null || _currentWeaponList[i] == null) continue;

            ItemsList[i].SetActive(true);
            
            TextMeshProUGUI[] TMPItemTitle = ItemsList[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[i].GetComponentInChildren<Button>(true);

            if (TMPItemTitle != null && TMPItemTitle.Length >= 2)
            {
                TMPItemTitle[0].text = _currentWeaponList[i].GetTextTitleInfo();
                TMPItemTitle[1].text = _currentWeaponList[i].GetTextDescriptionInfo();
            }
            
            int itemIndex = i;
            if (ItemButton != null)
            {
                ItemButton.onClick.RemoveAllListeners();
                ItemButton.onClick.AddListener(() => OnItemSelected(itemIndex));
            }
        }
        
    }

    private void OnItemSelected(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < _currentWeaponList.Count)
        {
            Weapon selectedWeapon = _currentWeaponList[itemIndex];
            if (selectedWeapon != null)
            {
                selectedWeapon.AddLevel(1);
            }

            if (levelUpController != null)
            {
                levelUpController.OnItemSelected(itemIndex);
            }
        }

        _currentWeaponList.Clear();
        
        // Обновляем UI после выбора
        SetItemsInfo();
    }
}