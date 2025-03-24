using System.Linq;
using UnityEngine;
using UnityEditor;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class Provider : MonoBehaviour
{
    [SerializeField, ValueDropdown(nameof(GetAllItemProviders)), OnCollectionChanged(nameof(HandleOnItemProviderChanged))] 
    protected ItemRequirementProvider[] itemProviders = null;
    
    [SerializeField, ValueDropdown(nameof(GetAllEnigmaEventProviders)), OnCollectionChanged(nameof(HandleOnEnigmaEventProviderChanged))] 
    protected EnigmaEventRequirementProvider[] enigmaEventProviders = null;
    
    private IEnumerable<ItemRequirementProvider> GetAllItemProviders() => Utilities.LoadResourcesInIEnumerable<ItemRequirementProvider>("ItemProviders");
    private IEnumerable<EnigmaEventRequirementProvider> GetAllEnigmaEventProviders() => Utilities.LoadResourcesInIEnumerable<EnigmaEventRequirementProvider>("EnigmaEventProviders");

    private void HandleOnItemProviderChanged()
    {
        if (itemProviders.Length <= 0)
            return;

        EditorApplication.delayCall += () => EditorGUIUtility.PingObject(itemProviders[0]);
    }
    private void HandleOnEnigmaEventProviderChanged()
    {
        if (enigmaEventProviders.Length <= 0)
            return;

        EditorApplication.delayCall += () => EditorGUIUtility.PingObject(enigmaEventProviders[0]);
    }

    public bool GetPermission()
    {
        return itemProviders.All(p => p.Permission) && enigmaEventProviders.All(p => p.Permission);
    }
}