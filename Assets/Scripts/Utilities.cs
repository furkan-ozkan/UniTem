using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public static class Utilities
{
    public static IEnumerable<ItemDataBase> GetAllItemDataBase() => LoadResourcesInIEnumerable<ItemDataBase>("ItemDatabase");
    public static IEnumerable<ItemRequirementProvider> GetAllItemProviders() => LoadResourcesInIEnumerable<ItemRequirementProvider>("ItemProviders");

    public static IEnumerable<T> LoadResourcesInIEnumerable<T>(string path) where T : Object
    {
        return Resources.LoadAll<T>(path).ToList();
    }
}