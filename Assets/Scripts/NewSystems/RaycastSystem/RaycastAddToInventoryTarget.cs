using UnityEngine;

[RequireComponent(typeof(Item))]
public sealed class RaycastAddToInventoryTarget : RaycastPlayerInputTargetListener
{
    private Item item = null;
    private PlayerInventory playerInventory = null;

    protected override void Awake()
    {
        base.Awake();
        item = GetComponent<Item>();
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    protected override void OnPlayerInputTargetPerformed()
    {
        base.OnPlayerInputTargetPerformed();
        playerInventory.GetInventory().AddItem(item);
    }
}