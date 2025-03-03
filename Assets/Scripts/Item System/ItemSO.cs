
using System;
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
    [FoldoutGroup("Basic Information",true)]
    public int ItemId;
    [FoldoutGroup("Basic Information")]
    public string ItemName;
    [FoldoutGroup("Basic Information")]
    public Sprite ItemIcon;
    [FoldoutGroup("Basic Information")]
    public GameObject ItemPrefab;

    [FoldoutGroup("Discovery Status",true)]
    [ShowInInspector, ReadOnly]
    public bool AlreadyFound { get; private set; }
    [FoldoutGroup("Discovery Status")]
    public BaseActionSO AlreadyFoundAction;

    [FoldoutGroup("Inventory Information",true)]
    [ShowInInspector, ReadOnly]
    public bool IsInInventory { get; private set; }

    [FoldoutGroup("Child Items",true)]
    public ItemSO[] ChildItems;

    [FoldoutGroup("Readable Panel",true)]
    public bool HasReadablePanel;
    [FoldoutGroup("Readable Panel")]
    public bool HasBeenRead { get; private set; }
    [FoldoutGroup("Readable Panel")]
    public BaseActionSO OnFirstReadAction;

    [FoldoutGroup("Audio and Events",true)]
    public AudioClip PickupSound;
    [FoldoutGroup("Audio and Events")]
    public BaseActionSO PickupAction;
    [FoldoutGroup("Audio and Events")]
    public AudioClip OnDropSound;
    [FoldoutGroup("Audio and Events")]
    public BaseActionSO OnDropAction;

    [FoldoutGroup("Consumability",true)]
    public bool IsConsumable;
    [FoldoutGroup("Consumability")]
    public bool IsConsumed { get; private set; }
    [FoldoutGroup("Consumability")]
    public BaseActionSO ConsumeAction;
    [FoldoutGroup("Consumability")]
    public AudioClip ConsumeSound;

    [FoldoutGroup("Transform Settings",true)]
    public Vector3 ItemHoldScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemHoldRotation;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceRotation;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemInspectScale;

    [FoldoutGroup("Item Type",true)]
    public ItemType itemType;

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