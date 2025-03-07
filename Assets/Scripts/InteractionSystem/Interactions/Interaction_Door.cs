using System;
using UnityEngine;

public class Interaction_Door : BaseInteractable
{
    private bool _isOpen = false;
    public Vector3 _openedRotation;
    public Vector3 _closedRotation;
    public float _rotationTime;
    
    public override bool Interact(GameObject player)
    {
        Context_Action_Rotate context = new Context_Action_Rotate(gameObject, _isOpen ? _closedRotation : _openedRotation, _rotationTime);
        if (CanInteract(context))
        {
            _actionInvoker.QueueAction(new Action_Rotate(),context);
            _isOpen = !_isOpen;
            return true;
        }

        return false;
    }

    public override void StartHover()
    {
        base.StartHover();
    }

    public override void EndHover()
    {
        base.EndHover();
    }
}