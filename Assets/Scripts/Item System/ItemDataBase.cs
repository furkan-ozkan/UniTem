using UnityEngine;

[CreateAssetMenu(fileName = "ItemDataBase", menuName = "ScriptableObjects/Item/ItemDataBase")]
public class ItemDataBase : ScriptableObject
{
    [SerializeField] private string itemName = string.Empty;
    public string ItemName => itemName;
}