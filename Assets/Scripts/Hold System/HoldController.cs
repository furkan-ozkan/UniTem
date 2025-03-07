using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class HoldController : MonoBehaviour
{
    public Transform holdParent;
    
    [Header("Debug Info")]
    [ShowInInspector, ReadOnly] private Transform heldItemObject;
    [ShowInInspector, ReadOnly] private Item heldItem;


    public void HoldItem(Transform item)
    {
        if (!heldItem)
        {
            InitializeHold(item);
            InitializeHoldPositioning();
        }
    }
    public void DropItem()
    {
        if (heldItem)
        {
            heldItem = null;
        }
    }
    
    private void InitializeHold(Transform item)
    {
        heldItemObject = item;
        heldItem = heldItemObject.GetComponent<Item>();
    }
    private void InitializeHoldPositioning()
    {
        heldItemObject.SetParent(holdParent);
        heldItemObject.DOMove(heldItem.itemData.ItemHoldPosition, 0.5f);
        heldItemObject.DOScale(heldItem.itemData.ItemHoldScale, 0.5f);
        heldItemObject.DORotate(heldItem.itemData.ItemHoldRotation, 0.5f);
    }
}