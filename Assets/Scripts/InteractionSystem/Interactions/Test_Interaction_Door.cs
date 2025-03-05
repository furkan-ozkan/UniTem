using UnityEngine;

public class Test_Interaction_Door : BaseInteractable
{
    private bool _isOpen = false;
    public ActionInvoker _actionInvoker; 
    public Vector3 _openedRotation;
    public Vector3 _closedRotation;
    public BaseActionSO _doorRotate;
    public override void Interact(ActionContext context)
    {
        Debug.Log("Interacted with door");
        context.ActionInteractable = InteractableId;
        context.Parameters.Add("OpenedRotation", _openedRotation);
        context.Parameters.Add("ClosedRotation", _closedRotation);
        context.Parameters.Add("IsOpen", _isOpen);
        
        _actionInvoker.AddToQueue(_doorRotate,context);
        _isOpen = !_isOpen;
    }
}