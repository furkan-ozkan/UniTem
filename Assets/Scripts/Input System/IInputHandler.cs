using UnityEngine;

public interface IInputHandler
{
    bool IsInteractPressed();
    Vector2 GetLookInput();
}