using UnityEngine;
using System.Collections.Generic;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "ItemDatabase", menuName = "Database/ItemDatabase")]
public class ItemDatabase : ScriptableObject
{
    private static ItemDatabase _instance;
    public static ItemDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ItemDatabase>("ScriptableObjects/Databases/ItemDatabase"); 

                if (_instance == null)
                {
                    Debug.LogError("ItemDatabase could not be found in Resources! Make sure it exists.");
                }
            }
            return _instance;
        }
    }

    [SerializeField, ReadOnly] private List<ItemSO> allItems = new List<ItemSO>();

    [Button("Refresh Item List")]
    public void LoadAllItems()
    {
        allItems = new List<ItemSO>(Resources.LoadAll<ItemSO>(""));
        Debug.Log($" ItemDatabase updated! Loaded {allItems.Count} items.");
    }

    public ItemSO GetItemById(int id)
    {
        return allItems.Find(item => item.ItemId == id);
    }
}