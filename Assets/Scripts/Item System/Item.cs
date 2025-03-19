using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class Item : BaseInteractable
{
    public ItemSO itemData;
    private Tween scaleTween; 
    private Tween moveTween; 
    private Tween rotationTween; 

    public override bool Interact(GameObject player)
    {
        ItemPickedUp(player);
        return true;
    }

    private void ItemPickedUp(GameObject player)
    {
        try
        {
            if (!itemData.ItemPrefab)
                itemData.ItemPrefab = gameObject;
            
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory == null)
                return;
        
            UpdateItemColliders(false);
            inventory.AddItem(this);
        
            UpdateItemScale(Vector3.zero);
        
            inventory.ChildInInventory(transform);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}\n{ex.StackTrace}");
        }
    }

    public void UpdateItemColliders(bool isEnabled)
    {
        Collider[] colliders = GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = isEnabled;
        }
    }

    public void UpdateItemScale(Vector3 scale)
    {
        scaleTween?.Complete();
        scaleTween = transform.DOScale(scale, .2f);
    }
    public void UpdateItemRotation(Vector3 rotation)
    {
        rotationTween?.Complete();
        rotationTween = transform.DORotate(rotation, .2f);
    }
    public void UpdateItemLocalPosition(Vector3 position)
    {
        moveTween?.Complete();
        moveTween = transform.DOLocalMove(position, .2f);
    }
    public void UpdateItemPosition(Vector3 position)
    {
        moveTween?.Complete();
        moveTween = transform.DOMove(position, .2f);
    }

    [Button("Fill Replace Settings")]
    public void FillHoldSettings()
    {
        itemData.ItemReplaceScale = transform.localScale;
        itemData.ItemReplaceRotation = transform.localEulerAngles;
    }
}