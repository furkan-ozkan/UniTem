using UnityEngine;

public class MockInputHandler : IInputHandler
{
    private bool interactPressed = false;
    private Vector2 lookInput = Vector2.zero;

    public void SetInteract(bool state) => interactPressed = state;
    public void SetLookInput(Vector2 value) => lookInput = value;

    public bool IsInteractPressed() => interactPressed;
    public Vector2 GetLookInput() => lookInput;
}