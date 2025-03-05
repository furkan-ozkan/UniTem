using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "ActionDatabase", menuName = "Database/ActionDatabase")]
public class ActionDatabase : ScriptableObject
{
    private static ActionDatabase _instance;
    public static ActionDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<ActionDatabase>("ScriptableObjects/Databases/ActionsDatabase");
                _instance.LoadAllActions();
            }
            return _instance;
        }
    }

    [SerializeField, ReadOnly] private List<BaseActionSO> allActions = new List<BaseActionSO>();

    [Button("Refresh Action List")]
    public void LoadAllActions()
    {
        allActions = new List<BaseActionSO>(Resources.LoadAll<BaseActionSO>(""));
    }

    public BaseActionSO GetActionById(int id)
    {
        return allActions.Find(action => action.ActionId == id);
    }
}