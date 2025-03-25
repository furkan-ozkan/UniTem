using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "ScriptableObjects/Item/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] private string itemName = string.Empty;
    private Item item = null;

    public string ItemName => itemName;
    public Item Item => item;

    public void Initialize(Item itemObject)
    {
        item = itemObject;
    }
}