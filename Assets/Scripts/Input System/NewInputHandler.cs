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

    public int? GetInventorySelection()
    {
        if (inputActions.Inventory.Slot1.WasPressedThisFrame())
            return 0;
        if (inputActions.Inventory.Slot2.WasPressedThisFrame())
            return 1;
        if (inputActions.Inventory.Slot3.WasPressedThisFrame())
            return 2;
        if (inputActions.Inventory.Slot4.WasPressedThisFrame())
            return 3;
        if (inputActions.Inventory.Slot5.WasPressedThisFrame())
            return 4;
    
        return null;
    }
}