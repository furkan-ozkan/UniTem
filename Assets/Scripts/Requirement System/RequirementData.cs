using System;
using Sirenix.OdinInspector;

[Serializable]
public class RequirementData
{
    [ValueDropdown(nameof(GetRequirementTypes))]
    public RequirementType requirementType;
    
    [ShowIf("requirementType", RequirementType.ItemRequirement)]
    public string itemName;
    
    [ShowIf("requirementType", RequirementType.EventRequirement)]
    public string eventName;

    private static RequirementType[] GetRequirementTypes()
    {
        return (RequirementType[])Enum.GetValues(typeof(RequirementType));
    }
}