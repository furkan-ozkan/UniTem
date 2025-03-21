using System;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private InventorySlotContainer inventory = null;
    private const string RESOURCES_PLAYER_INVENTORY_TEMPLATE_PATH = "Inventory/PlayerInventoryBase";

    private void Awake()
    {
        InventorySlotContainer playerInventoryTemplate = Resources.Load<InventorySlotContainer>(RESOURCES_PLAYER_INVENTORY_TEMPLATE_PATH);
        inventory = ScriptableObject.CreateInstance<InventorySlotContainer>();
        playerInventoryTemplate.GetCopy(ref inventory);
    }
    
    public InventorySlotContainer GetInventory() => inventory;
}