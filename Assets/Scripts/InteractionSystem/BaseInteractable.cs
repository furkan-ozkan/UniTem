using UnityEngine;
using Sirenix.OdinInspector;
using Knife.HDRPOutline.Core;
using System.Collections.Generic;
using UnityEditor;

[RequireComponent(typeof(ActionInvoker), typeof(Provider))]
public abstract class BaseInteractable : MonoBehaviour
{
    public string InteractableName { get; set; }

    [InlineProperty] [ListDrawerSettings(Expanded = true, ShowIndexLabels = true)] public List<DynamicRequirement> requirements = new List<DynamicRequirement>();

    public bool disableHoverAfterInteraction;
    public ActionInvoker _actionInvoker;
    protected Provider provider = null;


    private void Start()
    {
        _actionInvoker = GetComponent<ActionInvoker>();
        provider = GetComponent<Provider>();
    }

    public virtual bool Interact(GameObject player)
    {
        if (disableHoverAfterInteraction)
        {
            EndHover();
        }

        return true;
    }

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

    public bool RequirementsMet(GameObject player)
    {
        foreach (var req in requirements)
        {
            if (!req.IsMet(player)) return false;
        }

        return true;
    }
}