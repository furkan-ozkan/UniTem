using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemSO")]
public class ItemSO : ScriptableObject
{
    [FoldoutGroup("Basic Information", true)]
    [ShowInInspector, ReadOnly]
    [SerializeField] private int itemId;

    [FoldoutGroup("Basic Information")]
    public string ItemName;
    [FoldoutGroup("Basic Information")]
    public Sprite ItemIcon;
    [FoldoutGroup("Basic Information")]
    public GameObject ItemPrefab;

    [FoldoutGroup("Discovery Status", true)]
    [ShowInInspector]
    public bool AlreadyFound { get; set; }
    
    [FoldoutGroup("Inventory Information", true)]
    [ShowInInspector]
    public bool IsInInventory { get; private set; }

    [FoldoutGroup("Child Items", true)]
    public ItemSO[] ChildItems;

    [FoldoutGroup("Readable Panel", true)]
    public bool HasReadablePanel;
    [FoldoutGroup("Readable Panel")]
    [ShowInInspector]
    public bool HasBeenRead { get; private set; }

    [FoldoutGroup("Audio and Events", true)]
    public AudioClip PickupSound;
    [FoldoutGroup("Audio and Events")]
    public AudioClip OnDropSound;

    [FoldoutGroup("Consumability", true)]
    public bool IsConsumable;
    [FoldoutGroup("Consumability")]
    [ShowInInspector]
    public bool IsConsumed { get; private set; }
    [FoldoutGroup("Consumability")]
    public AudioClip ConsumeSound;

    [FoldoutGroup("Transform Settings", true)]
    public Vector3 ItemHoldPosition;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemHoldScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemHoldRotation;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceRotation;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemInspectScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemInspectRotation;

    public int ItemId => itemId;

    [Button("Fix Duplicate IDs")]
    public static void FixAllDuplicateIds()
    {
        ItemSO[] allItems = Resources.LoadAll<ItemSO>("");

        HashSet<int> usedIds = new HashSet<int>();
        List<ItemSO> duplicates = new List<ItemSO>();

        foreach (var item in allItems)
        {
            if (usedIds.Contains(item.itemId) || item.itemId == 0)
            {
                duplicates.Add(item);
            }
            else
            {
                usedIds.Add(item.itemId);
            }
        }

        foreach (var item in duplicates)
        {
            int newId = 1;
            while (usedIds.Contains(newId))
            {
                newId++;
            }

            Debug.LogWarning($"Fixing duplicate ID! Item {item.name} changed ID from {item.itemId} to {newId}");
            item.itemId = newId;
            usedIds.Add(newId);
        }

        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
    
    private void OnValidate()
    {
        ItemSO[] allItems = Resources.LoadAll<ItemSO>("");

        HashSet<int> usedIds = new HashSet<int>();
        foreach (var item in allItems)
        {
            if (item.itemId > 0)
                usedIds.Add(item.itemId);
        }

        if (itemId == 0 || IsDuplicateId(allItems, itemId))
        {
            int oldId = itemId;
            itemId = GenerateUniqueId(usedIds);

            if (oldId != 0)
                Debug.LogWarning($"Duplicate ID detected! Item {name} had ID {oldId}, changed to {itemId}");
            else
                Debug.Log($"Assigned new ID: {itemId} to item {name}");
        }
    }

    private bool IsDuplicateId(ItemSO[] allItems, int id)
    {
        int count = 0;
        foreach (var item in allItems)
        {
            if (item.itemId == id)
                count++;
        }
        return count > 1; 
    }

    private int GenerateUniqueId(HashSet<int> usedIds)
    {
        int newId = 1;

        while (usedIds.Contains(newId))
        {
            newId++;
        }

        usedIds.Add(newId);
        return newId;
    }
}
