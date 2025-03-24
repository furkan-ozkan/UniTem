using System;
using System.Linq;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;

public class EnigmaEventManager : MonoBehaviour
{
    [ShowInInspector] private static readonly Dictionary<EnigmaEventType, bool> enigmaEvents = new Dictionary<EnigmaEventType, bool>();

    private void Start()
    {
        enigmaEvents.Clear();
        foreach (EnigmaEventType enigmaEventType in Enum.GetValues(typeof(EnigmaEventType)))
        {
            enigmaEvents.Add(enigmaEventType, false);
        }
    }

    private void Update()
    {
        for (int i = 0; i <= 9; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha0 + i))
            {
                EnigmaEventType enigmaEventType = (EnigmaEventType)i;
                if (enigmaEvents.ContainsKey(enigmaEventType))
                {
                    enigmaEvents[enigmaEventType] = !enigmaEvents[enigmaEventType];
                }
            }
        }
    }

    public static bool GetEnigmaEventValue(EnigmaEventType eventType)
    {
        return enigmaEvents[eventType];
    }
    
    public static bool GetEnigmaEventsValue(EnigmaEventType[] eventTypes)
    {
        return eventTypes.All(GetEnigmaEventValue);
    }

    public static void SetEnigmaEventValue(EnigmaEventType type, bool value)
    {
        enigmaEvents[type] = value;
    }
}


public enum EnigmaEventType
{
    None = 0,
    SpeechToAndre = 1,
    SpeechToDiana = 2
}