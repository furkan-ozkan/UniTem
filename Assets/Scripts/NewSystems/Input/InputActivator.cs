using UnityEngine;
using UnityEngine.InputSystem;

public class InputActivator : MonoBehaviour
{
    [SerializeField] private InputActionAsset targetInputActionAsset = null;

    private void OnEnable() => EnableInputActionAsset();
    private void OnDisable() => DisableInputActionAsset();

    private void EnableInputActionAsset() => targetInputActionAsset.Enable();
    private void DisableInputActionAsset() => targetInputActionAsset.Disable();
}