using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private int maxCapacity = 10;
    [ShowInInspector, ReadOnly] private List<Item> items = new List<Item>();
    [SerializeField] private Item selectedItem;

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

    public bool AddItem(Item item)
    {
        if (IsFull())
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }
        
        if (item != null) 
        {
            items.Add(item);
            return true;
        }
        else 
        {
            return false;
        }
    }


    private void RemoveItem(GameObject item)
    {
        if (!item)
            return;
        
        items.Remove(item.GetComponent<Item>());
        item.transform.SetParent(null);
    }

    public void ClearInventory()
    {
        items.Clear();
    }

    public int GetCapacity()
    {
        return maxCapacity;
    }

    public int GetItemsCount()
    {
        return items.Count;
    }

    public List<Item> GetAllItems()
    {
        return items;
    }

    public Item GetItemByName(string itemName)
    {
        return items.Find(item => item.itemData.ItemName == itemName);
    }

    public void ChildInInventory(Transform item)
    {
        item.SetParent(inventoryParent);
        item.GetComponent<Item>().UpdateItemLocalPosition(Vector3.zero);
    }

    public bool IsFull()
    {
        if (GetItemsCount()>=GetCapacity())
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
        if (index < 0 || index >= items.Count)
            return;

        if (selectedItem == items[index])
        {
            PutSelectedItemInInventory();
        }
        else
        {
            PutSelectedItemInInventory();
            selectedItem = items[index];
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