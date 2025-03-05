using UnityEngine;

public abstract class BaseRequirement : ScriptableObject, IRequirement
{
    public int RequirementId;
    public abstract bool IsMet(ActionContext context);
}