using System;
using UnityEngine;
using Sirenix.OdinInspector;

[Serializable]
public class ItemSO
{
    [FoldoutGroup("Basic Information", true)]
    public string ItemName;
    [FoldoutGroup("Basic Information")]
    public Sprite ItemIcon;
    [FoldoutGroup("Basic Information")]
    public GameObject ItemPrefab;

    [FoldoutGroup("Discovery Status", true)]
    [ShowInInspector]
    public bool AlreadyFound { get; set; }

    [FoldoutGroup("InventorySlotContainer Information", true)]
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
    [Header("Hold Settings")]
    public Vector3 ItemHoldPosition;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemHoldScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemHoldRotation;
    [FoldoutGroup("Transform Settings")]
    [Header("Replace Settings")]
    public Vector3 ItemReplaceScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemReplaceRotation;
    [FoldoutGroup("Transform Settings")]
    [Header("Inspect Settings")]
    public Vector3 ItemInspectScale;
    [FoldoutGroup("Transform Settings")]
    public Vector3 ItemInspectRotation;
}