using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;

[CreateAssetMenu(fileName = "InteractableDatabase", menuName = "Database/InteractableDatabase")]
public class InteractableDatabase : ScriptableObject
{
    private static InteractableDatabase _instance;
    public static InteractableDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<InteractableDatabase>("ScriptableObjects/Databases/InteractableDatabases");
            }
            return _instance;
        }
    }

    [SerializeField, ReadOnly] private List<BaseInteractable> allInteractables = new List<BaseInteractable>();

    [Button("Refresh Interactable List (Manual)")]
    public void RefreshInteractableList()
    {
        var foundInteractables = FindObjectsOfType<BaseInteractable>().ToList();
        foreach (var interactable in foundInteractables)
        {
            if (!allInteractables.Contains(interactable))
                allInteractables.Add(interactable);
        }
        
        allInteractables.RemoveAll(interactable => interactable == null);
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void AutoUpdateOnSceneLoad()
    {
        if (Instance != null)
        {
            Instance.RefreshInteractableList();
        }
    }
    
    public BaseInteractable GetInteractableById(int id)
    {
        return allInteractables.Find(interactable => interactable.InteractableId == id);
    }
    
    public void CleanupNullReferences()
    {
        allInteractables.RemoveAll(interactable => interactable == null);
    }
}
