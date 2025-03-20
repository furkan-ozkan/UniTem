using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

namespace RedAxeGames
{
    [RequireComponent(typeof(RaycastTarget))]
    public class RaycastPlayerInputTarget : RaycastTargetListener
    {
        [SerializeField] private UnityEvent onInputPerformed = null;
        [SerializeField] private InputActionReference input = null;

        private float startInputTime = 0.00f;
        private float endInputTime = 0.00f;
        
        public static event System.Action<float, float> OnInputHolding = null;
        public static event System.Action OnHoldInputCanceled = null;
        
        private delegate void UpdateHandler();
        private event UpdateHandler updateCallback = null;
        
        private void Update()
        {
            updateCallback?.Invoke();
        }

        protected override void OnRaycastTargetMouseEnter()
        {
            updateCallback = null;
            input.action.Reset();
            input.action.started += OnInputStarted;
            input.action.performed += OnInputPerformed;
            input.action.canceled += OnInputCanceled;
        }

        protected override void OnRaycastTargetMouseExit()
        {
            updateCallback = null;
            input.action.started -= OnInputStarted;
            input.action.performed -= OnInputPerformed;
            input.action.canceled -= OnInputCanceled;
            OnHoldInputCanceled?.Invoke();
        }

        private void HandleInputHolding()
        {
            OnInputHolding?.Invoke(startInputTime, endInputTime);
        }
        
        
        private void OnInputStarted(InputAction.CallbackContext ctx)
        {
            if (ctx.interaction is HoldInteraction holdInteraction)
            {
                startInputTime = Time.time;
                endInputTime = startInputTime + (holdInteraction.duration.Equals(0.00f) ? InputSystem.settings.defaultHoldTime : holdInteraction.duration);

                updateCallback += HandleInputHolding;
            }
        }

        private void OnInputPerformed(InputAction.CallbackContext obj)
        {
            onInputPerformed?.Invoke();
        }

        private void OnInputCanceled(InputAction.CallbackContext obj)
        {
            if (obj.interaction is HoldInteraction _)
                OnHoldInputCanceled?.Invoke();

            updateCallback -= HandleInputHolding;
        }


    }
}