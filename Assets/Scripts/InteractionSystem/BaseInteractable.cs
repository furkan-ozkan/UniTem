using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[RequireComponent(typeof(ActionInvoker))]
public abstract class BaseInteractable : MonoBehaviour, IInteractable
{
    public string InteractableName { get; set; }

    [SerializeField, EnableIf("@manualIdEditMode")] 
    public int InteractableId;

    [SerializeField] private List<BaseRequirement> requirements = new List<BaseRequirement>();

    [FoldoutGroup("Debugging & Tools"), LabelText("Allow Manual ID Edit")]
    [Tooltip("Enable this if you want to manually change the ID.")]
    public bool manualIdEditMode = false;

    public virtual bool CanInteract(ActionContext context)
    {
        return requirements.All(requirement => requirement.IsMet(context));
    }


    public abstract void Interact(ActionContext context);

    private void OnValidate()
    {
        if (!manualIdEditMode)
        {
            ValidateAndAssignId();
        }
    }

    private void ValidateAndAssignId()
    {
        if (InteractableId == 0 || FindObjectsOfType<BaseInteractable>().Any(obj => obj != this && obj.InteractableId == InteractableId))
        {
            AssignUniqueId();
        }
    }

    private void AssignUniqueId()
    {
        HashSet<int> usedIds = new HashSet<int>(FindObjectsOfType<BaseInteractable>().Select(obj => obj.InteractableId));
        int newId = 1;

        while (usedIds.Contains(newId))
        {
            newId++;
        }

        InteractableId = newId;
        Debug.Log($"Assigned unique ID {InteractableId} to {gameObject.name}");
    }

    [Button("Check Duplicate IDs"), FoldoutGroup("Debugging & Tools")]
    private void CheckDuplicateIds()
    {
        var interactables = FindObjectsOfType<BaseInteractable>();
        var duplicateGroups = interactables.GroupBy(x => x.InteractableId)
                                           .Where(g => g.Count() > 1)
                                           .ToList();

        if (duplicateGroups.Count == 0)
        {
            Debug.Log("No duplicate IDs found.");
        }
        else
        {
            Debug.LogError("Duplicate IDs detected!");
            foreach (var group in duplicateGroups)
            {
                string duplicates = string.Join(", ", group.Select(x => x.gameObject.name));
                Debug.LogError($"ID {group.Key} is used by: {duplicates}");
            }
        }
    }
}
