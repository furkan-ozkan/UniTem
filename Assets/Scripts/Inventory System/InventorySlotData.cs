using UnityEngine;

[System.Serializable]
public class InventorySlotData
{
    [SerializeField] private ItemDataBase item = null;
    public ItemDataBase Item => item;

    public bool IsEmpty => !item;

    public void AddItem<T>(T newItem) where T : ItemDataBase
    {
        item = newItem;
    }
}