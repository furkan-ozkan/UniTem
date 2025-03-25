using System;
using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField, HideInEditorMode] private InventorySlotContainer inventory = null;
    [SerializeField] private Transform itemsParent = null;

    private const string RESOURCES_PLAYER_INVENTORY_TEMPLATE_PATH = "Inventory/PlayerInventoryBase";

    private void OnEnable()
    {
        inventory.OnItemAdded += HandleInventoryOnOnItemAdded;
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= HandleInventoryOnOnItemAdded;
    }

    private void Awake()
    {
        InventorySlotContainer playerInventoryTemplate = Resources.Load<InventorySlotContainer>(RESOURCES_PLAYER_INVENTORY_TEMPLATE_PATH);
        inventory = ScriptableObject.CreateInstance<InventorySlotContainer>();
        playerInventoryTemplate.GetCopy(ref inventory);
    }

    public InventorySlotContainer GetInventory() => inventory;
    
    private void HandleInventoryOnOnItemAdded(InventorySlotData slotData)
    {
        Item newItem = slotData.ItemData.Item;
        GameObject itemObject = newItem.gameObject;
        
        itemObject.SetActive(false);
        itemObject.transform.SetParent(itemsParent);
        itemObject.transform.localPosition = Vector3.zero;
    }
}