using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private int maxCapacity = 10;
    [ShowInInspector, ReadOnly] private List<OldItem> oldItems = new List<OldItem>();
    [ShowInInspector, ReadOnly] private List<Item> items = new List<Item>();
    [SerializeField] private OldItem selectedItem;

    private void OnEnable()
    {
        InputProvider.OnEscPressed += PutSelectedItemInInventory;
        InputProvider.OnInventorySlotSelected += SelectItem;
        EventManager.OnItemReplaced += ReplaceItem;
    }

    private void OnDisable()
    {
        InputProvider.OnEscPressed -= PutSelectedItemInInventory;
        InputProvider.OnInventorySlotSelected -= SelectItem;
        EventManager.OnItemReplaced -= ReplaceItem;
    }

    public bool AddItem(OldItem item)
    {
        if (IsFull())
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }

        if (item != null)
        {
            oldItems.Add(item);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddItem(GameObject item)
    {
        if (!item)
            return;

        items.Add(item.GetComponent<Item>());
    }

    private void RemoveItem(GameObject item)
    {
        if (!item)
            return;

        oldItems.Remove(item.GetComponent<OldItem>());
        item.transform.SetParent(null);
    }

    public void ClearInventory()
    {
        oldItems.Clear();
    }

    public int GetCapacity()
    {
        return maxCapacity;
    }

    public int GetItemsCount()
    {
        return oldItems.Count;
    }

    public List<OldItem> GetAllItems()
    {
        return oldItems;
    }

    public OldItem GetItemByName(string itemName)
    {
        return oldItems.Find(item => item.itemData.ItemName == itemName);
    }

    public void ChildInInventory(Transform item)
    {
        item.SetParent(inventoryParent);
        item.GetComponent<OldItem>().UpdateItemLocalPosition(Vector3.zero);
    }

    public bool IsFull()
    {
        if (GetItemsCount() >= GetCapacity())
            return true;
        return false;
    }


    private void ReplaceItem()
    {
        if (selectedItem)
        {
            RemoveItem(selectedItem.gameObject);
            selectedItem = null;
        }
    }

    private void SelectItem(int index)
    {
        if (index < 0 || index >= oldItems.Count)
            return;

        if (selectedItem == oldItems[index])
        {
            PutSelectedItemInInventory();
        }
        else
        {
            PutSelectedItemInInventory();
            selectedItem = oldItems[index];
            selectedItem.UpdateItemScale(selectedItem.itemData.ItemHoldScale);
            selectedItem.UpdateItemLocalPosition(selectedItem.itemData.ItemHoldPosition);
            selectedItem.UpdateItemRotation(selectedItem.itemData.ItemHoldRotation);
            EventManager.ItemSelected(selectedItem.gameObject);
        }
    }

    public void PutSelectedItemInInventory()
    {
        selectedItem?.UpdateItemScale(Vector3.zero);
        selectedItem?.UpdateItemLocalPosition(Vector3.zero);
        selectedItem = null;
        EventManager.ClearSelectedItem();
    }
}