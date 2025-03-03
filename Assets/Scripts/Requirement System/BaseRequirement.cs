using UnityEngine;

public abstract class BaseRequirement : ScriptableObject, IRequirement
{
    public abstract bool IsMet(ActionContext context);
}