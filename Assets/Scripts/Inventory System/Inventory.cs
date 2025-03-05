using UnityEngine;
using System.Collections.Generic;

public class Inventory : MonoBehaviour
{
    [SerializeField] private int maxCapacity = 10;
    private List<int> itemIds = new List<int>(); // Artık ItemSO yerine sadece ItemID saklıyoruz.

    public bool AddItemById(int itemId)
    {
        if (itemId <= 0)
        {
            Debug.LogWarning("Invalid Item ID! Cannot add to inventory.");
            return false;
        }

        if (itemIds.Count >= maxCapacity)
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }

        ItemSO item = ItemDatabase.Instance.GetItemById(itemId);
        if (item != null) 
        {
            itemIds.Add(itemId);
            Debug.LogWarning($"Item with ID {itemId} added!");
            return true;
        }
        else 
        {
            Debug.LogWarning($"Item with ID {itemId} does not exist in the database!");
            return false;
        }
    }


    public bool RemoveItemById(int itemId)
    {
        if (itemId <= 0) 
        {
            Debug.LogWarning($"Invalid Item ID ({itemId})! Cannot remove from inventory.");
            return false;
        }

        if (!itemIds.Contains(itemId))
        {
            Debug.LogWarning($"Item with ID {itemId} is not in the inventory, cannot remove!");
            return false;
        }

        return itemIds.Remove(itemId);
    }


    public void ClearInventory()
    {
        itemIds.Clear();
    }

    public bool ContainsItemById(int itemId)
    {
        if (itemId <= 0) 
        {
            Debug.LogWarning("Invalid Item ID! Cannot contain in inventory.");
            return false;
        }
        return itemIds.Contains(itemId);
    }

    public int GetCapacity()
    {
        return maxCapacity;
    }

    public int GetItemsCount()
    {
        return itemIds.Count;
    }

    public List<ItemSO> GetAllItems()
    {
        List<ItemSO> items = new List<ItemSO>();
        foreach (int itemId in itemIds)
        {
            ItemSO item = ItemDatabase.Instance.GetItemById(itemId);
            if (item != null)
                items.Add(item);
        }
        return items;
    }
}