using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemRewardInfo : MonoBehaviour
{
    private List<Weapon> _currentWeaponList = new List<Weapon>();
    //private List<Weapon> weaponList = new List<Weapon>();

    public List<GameObject> ItemsList;
    private ItemDataSO _universalItem;
    private ItemDataSO _specialisedItem;
    public ItemType _questGiverType;



    public int numberOfCurrentWeaponList = 2;

    public ItemControllerSO _itemControllerSO;

    private void OnEnable()
    {
        SetItemsInfo();
    }


    public void TransferRandomObjects()
    {
       if(_itemControllerSO.itemUniversalPool != null)
        {
            int _randIndex = Random.Range(0, _itemControllerSO.itemUniversalPool.Count);
            _universalItem = _itemControllerSO.AllItemsPool[_randIndex];
        }
        if (_questGiverType == ItemType.Tecno)
        {
            if (_itemControllerSO.itemTecnoPool != null)
            {
                int _randIndex = Random.Range(0, _itemControllerSO.itemTecnoPool.Count);
                _specialisedItem = _itemControllerSO.AllItemsPool[_randIndex];
            }
        }
        else
            if (_itemControllerSO.itemMagicPool != null)
            {
                int _randIndex = Random.Range(0, _itemControllerSO.itemMagicPool.Count);
                _specialisedItem = _itemControllerSO.AllItemsPool[_randIndex];
            }

    }

    public void SetItemsInfo()
    {
        TransferRandomObjects();



        if (_universalItem != null)
        {
            TextMeshProUGUI[] TMPUniversalItemTitle = ItemsList[0].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[0].GetComponentInChildren<Button>(true);

            TMPUniversalItemTitle[0].text = _universalItem.itemTitle;
            TMPUniversalItemTitle[1].text = _universalItem.description;

        }

        if(_specialisedItem != null)
        {
            TextMeshProUGUI[] TMPSpecialisedItemTitle = ItemsList[1].GetComponentsInChildren<TextMeshProUGUI>(true);
            Button ItemButton = ItemsList[1].GetComponentInChildren<Button>(true);

            TMPSpecialisedItemTitle[0].text = _specialisedItem.itemTitle;
            TMPSpecialisedItemTitle[1].text = _specialisedItem.description;
        }




        //for (int i = 0; i < Mathf.Min(ItemsList.Count, _currentWeaponList.Count); i++)
        //{
        //    if (ItemsList[i] == null || _currentWeaponList[i] == null) continue;

        //    ItemsList[i].SetActive(true);

        //    TextMeshProUGUI[] TMPItemTitle = ItemsList[i].GetComponentsInChildren<TextMeshProUGUI>(true);
        //    Button ItemButton = ItemsList[i].GetComponentInChildren<Button>(true);

        //    if (TMPItemTitle != null && TMPItemTitle.Length >= 2)
        //    {
        //        Weapon weapon = _currentWeaponList[i];

        //        TMPItemTitle[0].text = weapon.GetTextTitleInfo();

        //        string levelInfo = $"Lvl {weapon.CurrentLevel}";
        //        if (weapon.CurrentLevel + 1 == weapon.weaponData.maxLevel)
        //        {
        //            TMPItemTitle[1].text = $"{levelInfo} → MAX LEVEL\n{weapon.GetUpgradeDescriptionForNextLevel()}";
        //        }
        //        else
        //        {
        //            TMPItemTitle[1].text = $"{levelInfo} → {weapon.CurrentLevel + 1}\n{weapon.GetUpgradeDescriptionForNextLevel()}";
        //        }
        //    }

        //    int itemIndex = i;
        //    if (ItemButton != null)
        //    {
        //        ItemButton.onClick.RemoveAllListeners();
        //        ItemButton.onClick.AddListener(() => OnItemSelected(itemIndex));
        //    }
        //}
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

        //if (levelUpController != null)
        //{
        //    levelUpController.OnItemSelected(itemIndex);
        //}
    }

    [ContextMenu("Force Refresh UI")]
    public void ForceRefreshUI()
    {
        SetItemsInfo();
    }
}