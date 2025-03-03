public interface IInteractable
{
    bool CanInteract(ActionContext context);
    void Interact();
}