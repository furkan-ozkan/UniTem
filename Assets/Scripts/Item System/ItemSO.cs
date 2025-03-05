using System;
using System.Collections.Generic;
using Sirenix.Serialization;
using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using UnityEngine;

[Flags]
public enum ItemType
{
    None = 0,
    Consumable = 1 << 0,
    QuestItem = 1 << 1,
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/ItemSO")]
public class ItemSO : ScriptableObject
{
    [FoldoutGroup("Basic Information", true)]
    [ShowInInspector, ReadOnly]
    [SerializeField] private int itemId; // ID elle girilemez, sadece okunabilir

    [FoldoutGroup("Basic Information")]
    public string ItemName;
    [FoldoutGroup("Basic Information")]
    public Sprite ItemIcon;
    [FoldoutGroup("Basic Information")]
    public GameObject ItemPrefab;

    [FoldoutGroup("Discovery Status", true)]
    [ShowInInspector, ReadOnly]
    public bool AlreadyFound { get; private set; }
    [FoldoutGroup("Discovery Status")]
    [ShowInInspector, SerializeReference]
    public IActionExecutable AlreadyFoundAction;

    [FoldoutGroup("Inventory Information", true)]
    [ShowInInspector, ReadOnly]
    public bool IsInInventory { get; private set; }

    [FoldoutGroup("Child Items", true)]
    public ItemSO[] ChildItems;

    [FoldoutGroup("Readable Panel", true)]
    public bool HasReadablePanel;
    [FoldoutGroup("Readable Panel")]
    public bool HasBeenRead { get; private set; }
    [FoldoutGroup("Readable Panel")]
    [ShowInInspector, SerializeReference]
    public IActionExecutable OnFirstReadAction;

    [FoldoutGroup("Audio and Events", true)]
    public AudioClip PickupSound;
    [FoldoutGroup("Audio and Events")]
    [ShowInInspector, SerializeReference]
    public IActionExecutable PickupAction;
    [FoldoutGroup("Audio and Events")]
    public AudioClip OnDropSound;
    [FoldoutGroup("Audio and Events")]
    [ShowInInspector, SerializeReference]
    public IActionExecutable OnDropAction;

    [FoldoutGroup("Consumability", true)]
    public bool IsConsumable;
    [FoldoutGroup("Consumability")]
    public bool IsConsumed { get; private set; }
    [FoldoutGroup("Consumability")]
    [ShowInInspector, SerializeReference]
    public IActionExecutable ConsumeAction;
    [FoldoutGroup("Consumability")]
    public AudioClip ConsumeSound;

    [FoldoutGroup("Transform Settings", true)]
    public Vector3 ItemHoldScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemHoldRotation;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceRotation;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemInspectScale;

    [FoldoutGroup("Item Type", true)]
    public ItemType itemType;

    public int ItemId => itemId; // ID dışarıdan değiştirilemez

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

        Debug.Log("✅ All duplicate IDs have been fixed!");
        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.AssetDatabase.Refresh();
    }
    
    private void OnValidate()
    {
        // **Tüm ItemSO nesnelerinin ID'lerini kontrol etmek için veritabanını yükle**
        ItemSO[] allItems = Resources.LoadAll<ItemSO>("");

        // **Tüm mevcut ID'leri takip et**
        HashSet<int> usedIds = new HashSet<int>();
        foreach (var item in allItems)
        {
            if (item.itemId > 0)
                usedIds.Add(item.itemId);
        }

        // **Eğer ID 0 ise veya başka bir itemle çakışıyorsa, yeni bir ID ata**
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
        return count > 1; // Aynı ID 2 veya daha fazla itemde varsa, çakışma vardır
    }

    private int GenerateUniqueId(HashSet<int> usedIds)
    {
        int newId = 1;

        // **Boşta olan en küçük ID'yi bul**
        while (usedIds.Contains(newId))
        {
            newId++;
        }

        usedIds.Add(newId);
        return newId;
    }

    public void MarkAsFound()
    {
        if (!AlreadyFound)
        {
            AlreadyFound = true;
            AlreadyFoundAction?.ExecuteAsync(new ActionContext()).Forget();
        }
    }

    public void MarkAsRead()
    {
        if (HasReadablePanel)
        {
            if (!HasBeenRead)
            {
                HasBeenRead = true;
                OnFirstReadAction?.ExecuteAsync(new ActionContext()).Forget();
            }
        }
    }

    public void PickUp()
    {
        if (!IsInInventory)
        {
            IsInInventory = true;
            PickupAction?.ExecuteAsync(new ActionContext()).Forget();
        }
    }

    public void Drop()
    {
        if (IsInInventory)
        {
            IsInInventory = false;
            OnDropAction?.ExecuteAsync(new ActionContext()).Forget();
        }
    }

    public void Consume()
    {
        if (IsConsumable && !IsConsumed)
        {
            IsConsumed = true;
            ConsumeAction?.ExecuteAsync(new ActionContext()).Forget();
        }
    }
}
