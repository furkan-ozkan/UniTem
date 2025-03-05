using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAction", menuName = "Actions/Test_Action_Door_Rotate")]
public class Test_Action_Door_Rotate : BaseActionSO
{
    protected override UniTask OnExecute(ActionContext context, CancellationToken cancellationToken)
    {
        Debug.Log("Executing Door Rotate Action");
        BaseInteractable door = InteractableDatabase.Instance.GetInteractableById(context.ActionInteractable);
        bool isOpen = context.GetParameter<bool>("IsOpen");
        if (isOpen)
        {
            door.transform.eulerAngles = context.GetParameter<Vector3>("OpenedRotation");
        }
        else
        {
            door.transform.eulerAngles = context.GetParameter<Vector3>("ClosedRotation");
        }
        return UniTask.CompletedTask;
    }
}