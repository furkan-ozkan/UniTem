using UnityEngine;

public class NewInputHandler : IInputHandler
{
    private PlayerInputActions inputActions;

    public NewInputHandler()
    {
        inputActions = new PlayerInputActions();
        inputActions.Enable();
    }

    public bool IsInteractPressed() => inputActions.Player.Interact.WasPressedThisFrame();
    public Vector2 GetLookInput() => inputActions.Player.Look.ReadValue<Vector2>();
}