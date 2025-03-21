using System;
using UnityEngine;

public class Interaction_Door : BaseInteractable
{
    private bool _isOpen = false;
    public Vector3 _openedRotation;
    public Vector3 _closedRotation;
    public float _rotationTime;

    public void TestInteract()
    {
        Debug.LogError(provider.Permission);
    }
    public override bool Interact(GameObject player)
    {
        base.Interact(player);
        Context_Action_Rotate context = new Context_Action_Rotate(gameObject, _isOpen ? _closedRotation : _openedRotation, _rotationTime);
        if (true)
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