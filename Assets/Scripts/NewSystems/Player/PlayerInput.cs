using UnityEngine;
using Sirenix.OdinInspector;

public class PlayerInput : MonoBehaviour
{
    #region Serialized Variables

    [TabGroup("Move Inputs")] public KeyCode SprintKey = KeyCode.LeftShift;
    [TabGroup("Interact Inputs")] public KeyCode InteractKey = KeyCode.Mouse0;
    [TabGroup("Interact Inputs")] public KeyCode UseKey = KeyCode.F;


    [TabGroup("Carry Inputs")] public KeyCode DropKey = KeyCode.Q;

    [TabGroup("Place Inputs")] public KeyCode PlaceStartKey = KeyCode.Mouse1;
    [TabGroup("Place Inputs")] public KeyCode PlaceCancelKey = KeyCode.Mouse1;
    [TabGroup("Place Inputs")] public KeyCode PlaceEndKey = KeyCode.Mouse0;

    [TabGroup("Place Inputs")] public KeyCode RotateSnapToggleKey = KeyCode.Z;

    #endregion
    #region Encapsulations
    public float Horizontal => Input.GetAxisRaw("Horizontal");
    public float Vertical => Input.GetAxisRaw("Vertical");
    #endregion
    #region Methods
    public bool KeyDown(KeyCode Key) => Input.GetKeyDown(Key);
    public bool KeyPressing(KeyCode Key) => Input.GetKey(Key);
    public bool KeyUp(KeyCode Key) => Input.GetKeyUp(Key);

    #endregion
}