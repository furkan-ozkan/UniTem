using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "RequirementDatabase", menuName = "Database/RequirementDatabase")]
public class RequirementDatabase : ScriptableObject
{
    private static RequirementDatabase _instance;
    public static RequirementDatabase Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<RequirementDatabase>("ScriptableObjects/Databases/RequirementsDatabases");
                _instance.LoadAllRequirements();
            }
            return _instance;
        }
    }

    [SerializeField, ReadOnly] private List<BaseRequirement> allRequirements = new List<BaseRequirement>();

    [Button("Refresh Requirement List")]
    public void LoadAllRequirements()
    {
        allRequirements = new List<BaseRequirement>(Resources.LoadAll<BaseRequirement>(""));
    }

    public BaseRequirement GetRequirementById(int id)
    {
        return allRequirements.Find(req => req.RequirementId == id);
    }
}