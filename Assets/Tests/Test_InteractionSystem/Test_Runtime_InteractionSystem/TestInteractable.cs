using UnityEngine;

public class TestInteractable : BaseInteractable
{
    public bool WasInteracted { get; private set; } = false;

    public bool CanInteract(ActionContext context)
    {
        throw new System.NotImplementedException();
    }

    public override void Interact()
    {
        WasInteracted = true; // Test sırasında bunun değiştiğini kontrol edeceğiz
        Debug.Log("TestInteractable interacted!");
    }
}