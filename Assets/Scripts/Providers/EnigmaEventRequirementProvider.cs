using System;
using UnityEngine;

[CreateAssetMenu(fileName = "EnigmaEvent", menuName = "ScriptableObjects/Providers/EnigmaEventProvider")]
public class EnigmaEventRequirementProvider : ProviderBase<EnigmaEventType>
{
    [SerializeField] private EnigmaEventType[] eventTypes = Array.Empty<EnigmaEventType>();
    public override bool Permission => EnigmaEventManager.GetEnigmaEventsValue(eventTypes);
}