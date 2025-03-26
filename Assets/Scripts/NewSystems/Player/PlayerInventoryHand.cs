using System;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(PlayerInventoryInput))]
public class PlayerInventoryHand : MonoBehaviour
{
    [SerializeField] private Transform inventoryItemsParent = null;
    private PlayerInventoryInput input = null;

    private void OnEnable()
    {
        input.OnInventorySlotChange += HandleOnInventorySlotChange;
    }

    private void OnDisable()
    {
        input.OnInventorySlotChange -= HandleOnInventorySlotChange;
    }

    private void Awake()
    {
        input = GetComponent<PlayerInventoryInput>();
    }

    private void HandleOnInventorySlotChange(int newSlotIndex)
    {
        ReleaseAllHandItems();
        
        if (inventoryItemsParent.childCount <= newSlotIndex)
            return;
        
        Item newHandItem = inventoryItemsParent.GetChild(newSlotIndex).GetComponent<Item>();
        newHandItem.gameObject.SetActive(true);
    }

    private void ReleaseAllHandItems()
    {
        Item[] handItems = inventoryItemsParent.GetComponentsInChildren<Item>();
        Array.ForEach(handItems, x => x.gameObject.SetActive(false));
    }
}