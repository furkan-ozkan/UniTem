using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "ScriptableObjects/Inventory/InventorySlotContainer", fileName = "New Inventory Container")]
public sealed class InventorySlotContainer : ScriptableObject
{
    [SerializeField] private List<InventorySlotData> slots = new List<InventorySlotData>();
    public int Capacity => slots.Count;
    
    public void GetCopy(ref InventorySlotContainer inventorySlotContainer)
    {
        inventorySlotContainer.slots = new List<InventorySlotData>(slots);
    }
    
    public void AddItem<T>(T newItem) where T : ItemDataBase
    {
        if (HasAvailableSlot(out InventorySlotData availableSlot))
        {
            availableSlot.AddItem(newItem);
            return;
        }
        
        Debug.LogError("Dont have any available slot");
    }

    private bool HasAvailableSlot(out InventorySlotData availableSlot)
    {
        availableSlot = slots.FirstOrDefault(x => x.IsEmpty);
        return availableSlot != null;
    }

    public bool HasItem<T>(T itemDataBase) where T : ItemDataBase
    {
        return slots.FirstOrDefault(x => x.Item.ItemName.Equals(itemDataBase.ItemName)) != null;
    }

    public bool HasItems<T>(T[] itemDataBases) where T : ItemDataBase
    {
        HashSet<string> itemNames = new HashSet<string>(slots.Select(x => x.Item.ItemName));
        return itemDataBases.All(item => itemNames.Contains(item.ItemName));
    }
}