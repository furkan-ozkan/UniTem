using System;
using UnityEngine;

public static class EventManager
{
    public static Action<GameObject> OnStartItemHold;
    public static Action OnEndItemHold;
    public static Action<GameObject> OnItemReplace;
    
    public static void StartItemHold(GameObject item) => OnStartItemHold?.Invoke(item);
    public static void EndItemHold() => OnEndItemHold?.Invoke();
    public static void ItemReplaced(GameObject item) => OnItemReplace?.Invoke(item);
}