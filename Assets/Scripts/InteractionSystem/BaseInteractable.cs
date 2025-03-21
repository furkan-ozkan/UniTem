using UnityEngine;
using Sirenix.OdinInspector;
using Knife.HDRPOutline.Core;
using System.Collections.Generic;

[RequireComponent(typeof(ActionInvoker))]
public abstract class BaseInteractable : MonoBehaviour
{
    public string InteractableName { get; set; }

    [InlineProperty] [ListDrawerSettings(Expanded = true, ShowIndexLabels = true)]
    public List<DynamicRequirement> requirements = new List<DynamicRequirement>();

    public bool disableHoverAfterInteraction;
    public ActionInvoker _actionInvoker;

    [SerializeField, ValueDropdown(nameof(GetAllProviders))]
    protected ItemRequirementProvider provider = null;

    private IEnumerable<ItemRequirementProvider> GetAllProviders() => Utilities.LoadResourcesInIEnumerable<ItemRequirementProvider>("ItemProviders");


    private void Start()
    {
        _actionInvoker = GetComponent<ActionInvoker>();
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