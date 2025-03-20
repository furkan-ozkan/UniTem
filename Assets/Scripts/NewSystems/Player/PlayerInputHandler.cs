using UnityEngine;
using RedAxeGames;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 NormalizedMovementInput => playerInput.Gameplay.Move.ReadValue<Vector2>().normalized;
    public Vector2 LookInput => playerInput.Gameplay.Look.ReadValue<Vector2>();

    [SerializeField] private InputActionAsset playerInputActionAsset = null;
    private PlayerInputs playerInput = null;
    private void OnEnable()
    {
        playerInput.Enable();
        playerInputActionAsset.Enable();
    }
    private void OnDisable()
    {
        playerInput.Disable();
        playerInputActionAsset.Disable();
    }
 
    private void Awake()
    {
        playerInput = new PlayerInputs();
    }
}