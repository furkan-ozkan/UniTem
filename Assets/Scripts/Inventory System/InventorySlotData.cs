using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    [SerializeField] private ItemDataBase itemData = null;
    public ItemDataBase ItemData => itemData;

    public bool IsEmpty => !itemData;

    public void AddItem<T>(T newItem) where T : ItemDataBase
    {
        itemData = newItem;
    }
}