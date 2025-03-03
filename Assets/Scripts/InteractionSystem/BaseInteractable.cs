using UnityEngine;
using System.Collections.Generic;

public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] private List<BaseRequirement> requirements = new List<BaseRequirement>();

    public virtual bool CanInteract(ActionContext context)
    {
        foreach (var requirement in requirements)
        {
            if (!requirement.IsMet(context))
                return false;
        }
        return true;
    }

    public abstract void Interact();
}