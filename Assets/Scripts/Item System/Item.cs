using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemData;

    public void MarkAsFound() => itemData.MarkAsFound();
    public void MarkAsRead() => itemData.MarkAsRead();
    public void PickUp() => itemData.PickUp();
    public void Drop() => itemData.Drop();
    public void Consume() => itemData.Consume();
}