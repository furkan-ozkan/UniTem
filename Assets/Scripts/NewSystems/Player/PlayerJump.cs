using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public class PlayerJump : MonoBehaviour
{
    #region Serialized Variables

    //[SerializeField, FoldoutGroup("Jump Variables")] private float _jumpSpeed = 0.00f;
    //[SerializeField, FoldoutGroup("Jump Variables")] private float _gravity = 20.00f;

    #endregion
    #region Private Variables

    //private PlayerInput _playerInput = null;
   // private CharacterController _characterController = null;

    #endregion
    #region Actions

   // public static event System.Action<float> OnJump = null;
   // public static event System.Action<float> OnAir = null;

    #endregion
    #region Methods
    private void Awake() {
        //_characterController = GetComponent<CharacterController>();
        //_playerInput = GetComponent<PlayerInput>();
    }
    private void Update() {
       /* if (_playerInput.KeyPressing(PlayerInput.J) && _characterController.isGrounded)
            OnJump?.Invoke(_jumpSpeed * Time.deltaTime);

        if (!_characterController.isGrounded)
            OnAir?.Invoke(_gravity * Time.deltaTime);*/
    }

    #endregion
}