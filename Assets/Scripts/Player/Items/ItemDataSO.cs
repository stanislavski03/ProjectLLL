using UnityEngine;
[CreateAssetMenu(fileName = "New Item Data", menuName = "Items/Item Data")]
public class ItemDataSO : ScriptableObject
{
    [Header("Basic Info")]
    public string itemTitle;
    public string description;
    public Sprite icon;

    [Header("Base Stats")]
    public bool isActiveItem;
    public bool isActive;
    public ItemType itemType;
    public bool HasOnEnemyDeathEvent;
    public bool HasOnSceneChangeEvent;
    public bool HasOnDamageGiveEvent;

    public ItemControllerSO ItemController;
    public virtual void OnPick()
    {
        ItemControllerSO.Instance.DistributeItem(this);
        isActive = true;
        ItemsPanel.Instance.SetItemsInfo();
        // ItemsPanel.Instance.ClearItemList();
    }

    public virtual void OnEnemyDeath(GameObject enemy)
    {
        Debug.Log("OnEnemyDeath");
    }
    
    public virtual void OnSceneChange()
    {
        Debug.Log("OnSceneChange");
    }

    public virtual void OnDamageGive()
    {
        Debug.Log("OnDamageGive");
    }

}
