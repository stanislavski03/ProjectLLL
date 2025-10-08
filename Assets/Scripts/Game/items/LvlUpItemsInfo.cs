using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
                
                // ПОДПИСЫВАЕМСЯ НА СОБЫТИЕ ПРОКАЧКИ ОРУЖИЯ
                weapon.OnLevelUp -= OnWeaponLevelUp;
                weapon.OnLevelUp += OnWeaponLevelUp;
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
                weapon.OnLevelUp -= OnWeaponLevelUp;
            }
        }
    }

    private void OnWeaponAvailableChanged()
    {
        SetWeaponList();
    }

    // НОВЫЙ МЕТОД: обрабатываем прокачку оружия
    private void OnWeaponLevelUp(Weapon weapon)
    {
        Debug.Log($"[LvlUpItemsInfo] Weapon {weapon.Data.weaponName} leveled up to {weapon.CurrentLevel}");
        
        // ОБНОВЛЯЕМ UI если меню прокачки еще открыто
        if (levelUpController != null)
        {
            SetItemsInfo();
        }
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
        
        // ЕСЛИ доступных оружий меньше чем нужно, берем все доступные
        if (weaponList.Count <= numberOfCurrentWeaponList)
        {
            _currentWeaponList.AddRange(weaponList);
            weaponList.Clear();
        }
        else
        {
            // Берем случайные оружия
            for (int i = 0; i < numberOfCurrentWeaponList; i++)
            {
                int randomIndex = UnityEngine.Random.Range(0, weaponList.Count);
                Weapon randomObject = weaponList[randomIndex];

                _currentWeaponList.Add(randomObject);
                weaponList.RemoveAt(randomIndex);
            }
        }
    }

    public void SetItemsInfo()
    {
        SetWeaponList();
        TransferRandomObjects();

        // Скрываем все элементы
        foreach (var item in ItemsList)
        {
            if (item != null) 
                item.SetActive(false);
        }

        // Показываем и настраиваем доступные элементы
        for (int i = 0; i < Mathf.Min(ItemsList.Count, _currentWeaponList.Count); i++)
        {
            if (ItemsList[i] == null || _currentWeaponList[i] == null) continue;

            ItemsList[i].SetActive(true);
            
            TextMeshProUGUI[] TMPItemTitle = ItemsList[i].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[i].GetComponentInChildren<Button>(true);

            if (TMPItemTitle != null && TMPItemTitle.Length >= 2)
            {
                Weapon weapon = _currentWeaponList[i];
                
                TMPItemTitle[0].text = weapon.GetTextTitleInfo();
                
                // ОБНОВЛЯЕМ ОПИСАНИЕ с информацией об уровне
                string levelInfo = $"Lvl {weapon.CurrentLevel}";
                if (weapon.IsMaxLevel)
                {
                    levelInfo = "MAX LEVEL";
                    TMPItemTitle[1].text = $"{levelInfo}\n{weapon.GetItemDescription()}";
                }
                else
                {
                    TMPItemTitle[1].text = $"{levelInfo} → {weapon.CurrentLevel + 1}\n{weapon.GetUpgradeDescriptionForNextLevel()}";
                }
            }
            
            int itemIndex = i;
            if (ItemButton != null)
            {
                ItemButton.onClick.RemoveAllListeners();
                ItemButton.onClick.AddListener(() => OnItemSelected(itemIndex));
                
                // БЛОКИРУЕМ кнопку если оружие на максимальном уровне
                Weapon weapon = _currentWeaponList[i];
                ItemButton.interactable = !weapon.IsMaxLevel;
            }
        }
    }

    private void OnItemSelected(int itemIndex)
    {
        if (itemIndex >= 0 && itemIndex < _currentWeaponList.Count)
        {
            Weapon selectedWeapon = _currentWeaponList[itemIndex];
            if (selectedWeapon != null && !selectedWeapon.IsMaxLevel)
            {
                Debug.Log($"[LvlUpItemsInfo] Leveling up: {selectedWeapon.Data.weaponName}");
                
                // ПРОКАЧЫВАЕМ ОРУЖИЕ
                selectedWeapon.AddLevel(1);
                
                // ЛОГИРУЕМ результат прокачки
                Debug.Log($"[LvlUpItemsInfo] After level up - Level: {selectedWeapon.CurrentLevel}, Damage: {selectedWeapon.GetDamage()}");
            }
        }

        // Вызываем колбэк выбора предмета
        if (levelUpController != null)
        {
            levelUpController.OnItemSelected(itemIndex);
        }
    }

    // ДОБАВИМ метод для принудительного обновления UI
    [ContextMenu("Force Refresh UI")]
    public void ForceRefreshUI()
    {
        SetItemsInfo();
    }
}