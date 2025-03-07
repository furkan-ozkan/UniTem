using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using System;

public class ItemManager : IDisposable
{
    public ItemManager()
    {
        EventManager.OnItemClicked += ItemPickedUp;
        EventManager.OnItemSelectedInInventory += ItemSelectedInInventory;
    }
    
    private async void ItemPickedUp(GameObject player, Item item)
    {
        try
        {
            Inventory inventory = player.GetComponent<Inventory>();
            if (inventory == null)
            {
                return;
            }
        
            UpdateItemColliders(item.gameObject, false);
            inventory.AddItemById(item);
        
            await UpdateItemScale(item.gameObject, Vector3.zero);
        
            inventory.ChildInInventory(item.transform);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}\n{ex.StackTrace}");
        }
    }

    private void ItemSelectedInInventory(GameObject player, Item clickedItem)
    {
        // Envanterde seçilen item ile ilgili işlemler burada yapılabilir.
    }

    private void UpdateItemColliders(GameObject item, bool isEnabled)
    {
        Collider[] colliders = item.GetComponentsInChildren<Collider>();
        foreach (Collider collider in colliders)
        {
            collider.enabled = isEnabled;
        }
    }

    private async UniTask UpdateItemScale(GameObject item, Vector3 scale)
    {
        await item.transform.DOScale(scale, .2f).ToUniTask();
    }

    public void Dispose()
    {
        EventManager.OnItemClicked -= ItemPickedUp;
        EventManager.OnItemSelectedInInventory -= ItemSelectedInInventory;
    }
}