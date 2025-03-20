using UnityEngine;
using Sirenix.OdinInspector;

// ReSharper disable All

[RequireComponent(typeof(PlayerInput), typeof(PlayerHand))]
public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactDistance = 0.00f;
    [SerializeField] private Transform _orientation = null;
    [SerializeField] private LayerMask _interactLayer = 0;
    [SerializeField] private LayerMask _lineCastLayer = 0;

    private PlayerInput _playerInput = null;
    private PlayerHand _playerHand = null;
    private bool _isInteracting = false;

    public bool IsInteracting
    {
        private get => _isInteracting;
        set => _isInteracting = value;
    }

    private bool _canInteract = true;

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(_orientation.position, _orientation.forward * _interactDistance, Color.green);
    }

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _playerHand = GetComponent<PlayerHand>();
    }

    private void OnEnable()
    {
        PlayerHandPlaceState.OnPlaceEnd += () =>
        {
            DisableCanInteract();
            Invoke(nameof(EnableCanInteract), 0.15f);
        };
    }

    private void OnDisable()
    {
        PlayerHandPlaceState.OnPlaceEnd -= () =>
        {
            DisableCanInteract();
            Invoke(nameof(EnableCanInteract), 0.15f);
        };
    }

    private void Update()
    {
        if (_playerHand.IsCurrentState<PlayerHandBlockedState>())
            return;

        CheckRay();
    }

    private void CheckRay()
    {
        if (_playerHand.IsCurrentState<PlayerHandPlaceState>())
        {
            return;
        }

        if (SendRay(out RaycastHit RayHit))
        {
            if (Physics.Linecast(transform.position, RayHit.point, _lineCastLayer))
            {
                return;
            }

            return;
        }
    }

    private bool SendRay(out RaycastHit Hit) => Physics.Raycast(_orientation.position, _orientation.forward, out Hit, _interactDistance, _interactLayer);


    private void SetCanInteract(bool Value) => _canInteract = Value;

    private void EnableCanInteract() => SetCanInteract(true);
    private void DisableCanInteract() => SetCanInteract(false);
}