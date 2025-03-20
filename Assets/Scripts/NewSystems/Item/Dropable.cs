using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.InputSystem;

namespace RedAxeGames
{
    [RequireComponent(typeof(Carryable))]
    public class Dropable : MonoBehaviour
    {
        [SerializeField] private float dropForce = 5.00f;
        [SerializeField] private InputAction dropAction = null;

        private Rigidbody rigidBody = null;
        private Collider collider = null;
        private Carryable carryable = null;

        private void OnEnable()
        {
            carryable.OnCarriedEvent.AddListener(HandleOnCarryableCarried);
        }

        private void OnDisable()
        {
            carryable.OnCarriedEvent.RemoveListener(HandleOnCarryableCarried);
        }

        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            carryable = GetComponent<Carryable>();
        }

        private void HandleOnCarryableCarried()
        {
            if (!dropAction.enabled)
            {
                dropAction.Reset();
                
                dropAction.Enable();
                dropAction.performed += OnDropInputPerformed;
            }
        }

        private void OnDropInputPerformed(InputAction.CallbackContext ctx)
        {
            if (dropAction.enabled)
            {
                UniTask.DelayFrame(1).ContinueWith(delegate
                {
                    dropAction.performed -= OnDropInputPerformed;
                    dropAction.Disable();
                }).Forget();
            }

            Drop();
        }

        public void Drop()
        {
            Vector3 ForceDirection = transform.root.forward * dropForce;
            transform.SetParent(null);

            collider.isTrigger = false;
            rigidBody.isKinematic = false;
            rigidBody.AddForce(ForceDirection, ForceMode.Impulse);
        }
    }
}