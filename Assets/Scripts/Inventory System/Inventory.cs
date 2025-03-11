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
    [SerializeField, ReadOnly] private bool _canSelectItem = true;

    private void OnEnable()
    {
        InputProvider.OnInventorySlotSelected += GetItemInInventoryByIndex;
        InputProvider.OnEscPressed += EndSelectItem;
        EventManager.OnItemReplace += RemoveItem;
    }

    private void OnDisable()
    {
        InputProvider.OnInventorySlotSelected -= GetItemInInventoryByIndex;
        InputProvider.OnEscPressed -= EndSelectItem;
        EventManager.OnItemReplace -= RemoveItem;
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


    public void RemoveItem(GameObject item)
    {
        if (!item)
            return;
        
        items.Remove(item.GetComponent<Item>());
        item.transform.SetParent(null);
        if (selectedItem == item.GetComponent<Item>())
            selectedItem = null;
    }

    public void EndSelectItem()
    {
        selectedItem?.UpdateItemScale(Vector3.zero);
        selectedItem?.UpdateItemLocalPosition(Vector3.zero);
        selectedItem = null;
        EventManager.EndItemHold();
    }

    public void GetItemInInventoryByIndex(int inventoryIndex)
    {
        if (!_canSelectItem || inventoryIndex >= items.Count) return;
        
        CanSelectCD().Forget();
        EndSelectItem();
        if (selectedItem == items[inventoryIndex]) return;
        
        selectedItem = items[inventoryIndex];
        selectedItem.UpdateItemScale(selectedItem.itemData.ItemHoldScale);
        selectedItem.UpdateItemLocalPosition(selectedItem.itemData.ItemHoldPosition);
        
        EventManager.StartItemHold(items[inventoryIndex].gameObject);
    }

    private async UniTask CanSelectCD()
    {
        _canSelectItem = false;
        await Task.Delay(200);
        _canSelectItem = true;
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
}