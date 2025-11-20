using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class LvlUpWeaponItemsInfo : MonoBehaviour
{
    public List<Weapon> _currentWeaponList = new List<Weapon>();
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
            if (allWeaponList[i] != null && allWeaponList[i].IsAvailable && !allWeaponList[i].IsMaxLevel)
            {
                weaponList.Add(allWeaponList[i]);
            }
        }
    }

    public void TransferRandomObjects()
    {
        _currentWeaponList.Clear();

        // Если нет доступных оружий, выходим
        if (weaponList.Count == 0)
        {
            Debug.LogWarning("No weapons available in weaponList!");
            return;
        }

        // ЕСЛИ доступных оружий меньше или равно чем нужно, берем все доступные
        if (weaponList.Count <= numberOfCurrentWeaponList)
        {
            // Перемешиваем список перед добавлением
            ShuffleWeaponList();
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

    // Метод для перемешивания списка оружий
    private void ShuffleWeaponList()
    {
        for (int i = 0; i < weaponList.Count; i++)
        {
            Weapon temp = weaponList[i];
            int randomIndex = UnityEngine.Random.Range(i, weaponList.Count);
            weaponList[i] = weaponList[randomIndex];
            weaponList[randomIndex] = temp;
        }
    }

    public void SetItemsInfo()
    {
        SetWeaponList();
        TransferRandomObjects();
        WeaponsPanel.Instance.SetWeaponsInfo();

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
            Transform imageTransform = ItemsList[i].transform.Find("Image");
            Image ItemImage = imageTransform?.GetComponent<Image>();
            Button ItemButton = ItemsList[i].GetComponentInChildren<Button>(true);

            Weapon weapon = _currentWeaponList[i];

            if (ItemImage != null)
            {
                ItemImage.preserveAspect = true;
                ItemImage.type = Image.Type.Simple;

                RectTransform rectTransform = ItemImage.GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    rectTransform.sizeDelta = new Vector2(100, 100);
                }

                ItemImage.sprite = weapon.weaponData.icon;
            }

            if (TMPItemTitle != null && TMPItemTitle.Length >= 2)
            {

                TMPItemTitle[0].text = weapon.GetTextTitleInfo();

                string levelInfo = $"Lvl {weapon.CurrentLevel}";
                if (weapon.CurrentLevel + 1 == weapon.weaponData.maxLevel)
                {
                    TMPItemTitle[1].text = $"{levelInfo} → MAX LEVEL\n{weapon.GetUpgradeDescriptionForNextLevel()}";
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
                selectedWeapon.AddLevel(1);
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