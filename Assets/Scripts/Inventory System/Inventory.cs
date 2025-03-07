using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Sirenix.OdinInspector;

public class Inventory : MonoBehaviour
{
    [SerializeField] private Transform inventoryParent;
    [SerializeField] private int maxCapacity = 10;
    [ShowInInspector, ReadOnly] private List<Item> items = new List<Item>(); // Artık ItemSO yerine sadece ItemID saklıyoruz.

    public bool AddItemById(Item item)
    {
        if (item.itemData.ItemId <= 0)
        {
            Debug.LogWarning("Invalid Item ID! Cannot add to inventory.");
            return false;
        }

        if (items.Count >= maxCapacity)
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


    public bool RemoveItemById(int itemId)
    {
        if (itemId <= 0) 
        {
            Debug.LogWarning($"Invalid Item ID ({itemId})! Cannot remove from inventory.");
            return false;
        }

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].itemData.ItemId == itemId)
            {
                items.RemoveAt(i);
                return true;
            }
        }

        return false;
    }



    public void ClearInventory()
    {
        items.Clear();
    }

    public bool ContainsItemById(int itemId)
    {
        if (itemId <= 0) 
        {
            Debug.LogWarning("Invalid Item ID! Cannot contain in inventory.");
            return false;
        }
    
        return items.Any(m_Item => m_Item.itemData.ItemId == itemId);
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
        item.transform.localScale = Vector3.zero;
    }

    public bool IsFull()
    {
        if (GetItemsCount()>=GetCapacity())
            return true;
        return false;
    }
}