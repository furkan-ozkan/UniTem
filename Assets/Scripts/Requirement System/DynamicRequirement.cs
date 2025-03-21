using System;
using Sirenix.OdinInspector;
using UnityEngine;

[Serializable]
public class DynamicRequirement
{
    [ValueDropdown(nameof(GetRequirementTypes))] 
    public RequirementType requirementType;

    [ShowIf("requirementType", RequirementType.ItemRequirement)]
    public string itemName;
    
    [ShowIf("@requirementType == RequirementType.EventRequirement || requirementType == RequirementType.UncompletedRequirement")]
    public string eventName; 

    private static RequirementType[] GetRequirementTypes()
    {
        return (RequirementType[])Enum.GetValues(typeof(RequirementType));
    }

    public bool IsMet(GameObject player)
    {
        // switch (requirementType)
        // {
        //     case RequirementType.ItemRequirement:
        //         return player.GetComponent<InventorySlotContainer>().GetItemByName(itemName);
        //     
        //     case RequirementType.EventRequirement:
        //         return EventManager.HasEventOccurred(eventName);
        //         
        //     case RequirementType.UncompletedRequirement:
        //         return !EventManager.HasEventOccurred(eventName);
        // }
        return false;
    }
}

public enum RequirementType
{
    ItemRequirement,
    EventRequirement,
    UncompletedRequirement
}