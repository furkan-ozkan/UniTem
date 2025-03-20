using UnityEngine;
using UnityEngine.InputSystem;

namespace RedAxeGames
{

    public class OldPlayerInputHandler : MonoBehaviour, IInputAction
    {
        private PlayerInputs playerInputActions;

        private void OnEnable()
        {
            playerInputActions = InputManager.PlayerInputs;
            InputManager.SetInputEnability(InputManager.InputMapType.Gameplay, true);

            // playerInputActions.Gameplay.Move.performed += OnMovePerformed;
            // playerInputActions.Gameplay.Jump.performed += OnJumpPerformed;
            // playerInputActions.Gameplay.Interaction.performed += OnInteracitonPerformed;
        }

        private void OnDisable()
        {
            InputManager.SetInputEnability(InputManager.InputMapType.Gameplay, false);
            // playerInputActions.Gameplay.Move.performed -= OnMovePerformed;
            // playerInputActions.Gameplay.Jump.performed -= OnJumpPerformed;
            // playerInputActions.Gameplay.Interaction.performed -= OnInteracitonPerformed;

            playerInputActions.Disable();
        }

        private void OnMovePerformed(InputAction.CallbackContext context)
        {
            OnMove(context.ReadValue<Vector2>());
        }

        private void OnJumpPerformed(InputAction.CallbackContext context)
        {
            OnJump();
        }

        // private void OnInteracitonPerformed(InputAction.CallbackContext context)
        // {
        //     OnInteract();
        // }

        public void OnMove(Vector2 direction)
        {
            // Implement movement logic here
        }

        public void OnJump()
        {
            // Implement jump logic here
        }
    }
}