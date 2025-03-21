using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

[Serializable]
[CreateAssetMenu(menuName = "ScriptableObjects/Providers/ItemProvider")]
public sealed class ItemRequirementProvider : ProviderBase<ItemDataBase>
{
    [SerializeField, ValueDropdown(nameof(GetAllItemDatabases))]
    private ItemDataBase[] requirementItem = Array.Empty<ItemDataBase>();

    public void SetInventory<T>(ref T inventoryRef) where T : PlayerInventory => _playerInventory = inventoryRef;
    private PlayerInventory _playerInventory = null;

    private IEnumerable<ItemDataBase> GetAllItemDatabases() => Utilities.LoadResourcesInIEnumerable<ItemDataBase>("ItemDatabase");
    public override bool Permission => _playerInventory.GetInventory().HasItems(requirementItem);
}