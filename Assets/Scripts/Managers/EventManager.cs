using System;
using UnityEngine;

public static class EventManager
{
    public static Action<GameObject> OnItemSelected;
    public static Action OnClearSelectedItem;
    public static Action<GameObject> OnItemReplaceClicked;
    public static Action OnItemReplaced;
    
    public static void ItemSelected(GameObject item) => OnItemSelected?.Invoke(item);
    public static void ClearSelectedItem() => OnClearSelectedItem?.Invoke();
    public static void ItemReplaceClicked(GameObject item) => OnItemReplaceClicked?.Invoke(item);
    public static void ItemReplaced() => OnItemReplaced?.Invoke();
}