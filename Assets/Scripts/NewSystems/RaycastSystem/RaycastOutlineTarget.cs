using RedAxeGames;
using UnityEngine;
using Sirenix.OdinInspector;
using Knife.HDRPOutline.Core;

[RequireComponent(typeof(RaycastTarget))]
public class RaycastOutlineTarget : RaycastTargetListener
{
    [SerializeField] private OutlineActivateType defaultOutlineActivateType = OutlineActivateType.Inactive;
    [Space(20.00f)]
    [SerializeField] private RaycastOutlineTargetConfig[] configs = System.Array.Empty<RaycastOutlineTargetConfig>();
    private OutlineObject[] outlinables = null;

    protected override void Awake()
    {
        base.Awake();
        outlinables = GetComponentsInChildren<OutlineObject>();
        ChangeActivateOutline(defaultOutlineActivateType);
    }

    [Button]
    private void ActivateOutline()
    {
        ChangeActivateOutline(OutlineActivateType.Active);
    }

    [Button]
    private void InactivateOutline()
    {
        ChangeActivateOutline(OutlineActivateType.Inactive);
    }

    private void ChangeActivateOutline(OutlineActivateType activateType)
    {
        outlinables ??= GetComponentsInChildren<OutlineObject>();
        System.Array.ForEach(outlinables,
            x => x.enabled = activateType.Equals(OutlineActivateType.Active));
    }

    protected override void OnRaycastTargetMouseEnter()
    {
        foreach (RaycastOutlineTargetConfig config in configs)
        {
            if (config.TargetOutlineEventType.Equals(OutlineEventType.OnMouseEnter))
                ChangeActivateOutline(config.TargetOutlineActivateType);
        }
    }

    protected override void OnRaycastTargetMouseExit()
    {
        foreach (RaycastOutlineTargetConfig config in configs)
        {
            if (config.TargetOutlineEventType.Equals(OutlineEventType.OnMouseExit))
                ChangeActivateOutline(config.TargetOutlineActivateType);
        }
    }
}

public enum OutlineEventType
{
    OnMouseEnter = 0,
    OnMouseExit = 1,
}

public enum OutlineActivateType
{
    Inactive = 0,
    Active = 1,
}

[System.Serializable]
public class RaycastOutlineTargetConfig
{
    [SerializeField, HideLabel, HorizontalGroup("Horizontal")]
    private OutlineEventType targetOutlineEventType = OutlineEventType.OnMouseEnter;

    [SerializeField, HideLabel, HorizontalGroup("Horizontal")]
    private OutlineActivateType _outlineType = OutlineActivateType.Inactive;

    public OutlineEventType TargetOutlineEventType => targetOutlineEventType;
    public OutlineActivateType TargetOutlineActivateType => _outlineType;
}