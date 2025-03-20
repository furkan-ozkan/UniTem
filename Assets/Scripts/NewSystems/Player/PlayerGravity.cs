using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerGravity : MonoBehaviour
{
    [SerializeField] private float _gravity = 0.00f;
    private CharacterController _characterController = null;

    private void Awake() {
        _characterController = GetComponent<CharacterController>();
    }

    private void LateUpdate() {
        if (_characterController.isGrounded)
            return;

        _characterController.Move(_gravity * Time.deltaTime * Vector3.down);
    }
}