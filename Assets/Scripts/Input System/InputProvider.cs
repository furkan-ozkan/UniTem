using UnityEngine;
using System;

public static class InputProvider
{
    private static PlayerInputActions inputActions;
    private static bool isInitialized = false;

    public static event Action<int> OnInventorySlotSelected;
    public static event Action OnInteractPressed;
    public static event Action<Vector2> OnLookInput;
    public static event Action OnEscPressed;

    public static void Initialize()
    {
        if (isInitialized) return;

        inputActions = new PlayerInputActions();
        inputActions.Enable();

        inputActions.Inventory.Slot1.performed += ctx => OnInventorySlotSelected?.Invoke(0);
        inputActions.Inventory.Slot2.performed += ctx => OnInventorySlotSelected?.Invoke(1);
        inputActions.Inventory.Slot3.performed += ctx => OnInventorySlotSelected?.Invoke(2);
        inputActions.Inventory.Slot4.performed += ctx => OnInventorySlotSelected?.Invoke(3);
        inputActions.Inventory.Slot5.performed += ctx => OnInventorySlotSelected?.Invoke(4);

        inputActions.Player.Interact.performed += ctx => OnInteractPressed?.Invoke();
        inputActions.Player.Look.performed += ctx => OnLookInput?.Invoke(ctx.ReadValue<Vector2>());
        inputActions.Inventory.ESC.performed += ctx => OnEscPressed?.Invoke();

        isInitialized = true;
    }
}