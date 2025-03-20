using System;
using System.Collections.Generic;
using UnityEngine;

public static class EventManager
{
    private static HashSet<string> occurredEvents = new HashSet<string>();
    
    /// <summary>
    /// OldItem Hold And Replace Events
    /// </summary>
    public static Action<GameObject> OnItemSelected;
    public static Action OnClearSelectedItem;
    public static Action OnItemReplaced;
    
    public static void ItemSelected(GameObject item) => OnItemSelected?.Invoke(item);
    public static void ClearSelectedItem() => OnClearSelectedItem?.Invoke();
    public static void ItemReplaced() => OnItemReplaced?.Invoke();
    
    
    public static void RecordEvent(string eventName)
    {
        if (!occurredEvents.Contains(eventName))
        {
            occurredEvents.Add(eventName);
            Debug.Log("Event kaydedildi: " + eventName);
        }
    }

    public static bool HasEventOccurred(string eventName)
    {
        return occurredEvents.Contains(eventName);
    }
}