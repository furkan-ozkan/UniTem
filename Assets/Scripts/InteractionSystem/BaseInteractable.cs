using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

// required component outlineobject ve target
public abstract class BaseInteractable : MonoBehaviour
{
    public string InteractableName { get; set; }
    // outline object ve target al
    [SerializeField] private List<BaseRequirement> requirements = new List<BaseRequirement>();

    public virtual bool CanInteract()
    {
        return requirements.All(requirement => requirement.IsMet());
    }
    
    public abstract UniTask Interact();

    // outline objecti ve targeti  ac kapa fonksiyonlari
}
