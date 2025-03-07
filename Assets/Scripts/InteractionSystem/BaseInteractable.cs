using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Knife.HDRPOutline.Core;

[RequireComponent(typeof(ActionInvoker))]
public abstract class BaseInteractable : MonoBehaviour
{
    public string InteractableName { get; set; }
    public ActionInvoker _actionInvoker; 

    [SerializeField] private List<BaseRequirement> requirements = new List<BaseRequirement>();
    
    private void Start()
    {
        _actionInvoker = GetComponent<ActionInvoker>();
    }

    public virtual bool CanInteract(ActionContext context)
    {
        return requirements.All(requirement => requirement.IsMet(context));
    }
    
    public abstract bool Interact(GameObject player);

    public virtual void StartHover()
    {
        foreach (var outline in GetComponentsInChildren<OutlineObject>())
        {
            outline.enabled = true;
        }
    }

    public virtual void EndHover()
    {
        foreach (var outline in GetComponentsInChildren<OutlineObject>())
        {
            outline.enabled = false;
        }
    }
}
