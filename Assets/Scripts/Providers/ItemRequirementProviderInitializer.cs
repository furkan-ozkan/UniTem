using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class ItemRequirementProviderInitializer : MonoBehaviour
{
    private List<ItemRequirementProvider> providers = new List<ItemRequirementProvider>();
    private PlayerInventory _playerInventory = null;

    private void Start()
    {
        _playerInventory = FindObjectOfType<PlayerInventory>();
        providers.AddRange(Utilities.LoadResourcesInIEnumerable<ItemRequirementProvider>("ItemProviders").ToList());
        
        providers.ForEach(x=>x.SetInventory(ref _playerInventory));
    }
}
