using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(CharacterController), typeof(PlayerInput))]
public sealed class PlayerMove : AbstractStateMachine<AbstractPlayerMovementState>
{
    #region Serialized Variables

    [SerializeField, FoldoutGroup("Movement Variables")] private float _moveSpeed = 0.00f;
    [SerializeField, FoldoutGroup("Movement Variables")] private float _sprintSpeed = 0.00f;

    #endregion
    #region Private Variables
    private PlayerInput _input = null;
    private CharacterController _characterController = null;


    private PlayerIdleState _idleState = null;
    private PlayerMoveState _moveState = null;
    private PlayerStunState _stunState = null;

    #endregion
    #region Encapsulations

    public float MoveSpeed => _moveSpeed;
    public float SprintSpeed => _sprintSpeed;

    public CharacterController Controller => _characterController;

    protected override AbstractPlayerMovementState InitializeState => _idleState;

    #endregion
    #region Methods

    private void OnEnable() {
        _idleState.OnRequestExit += () => SetState(_moveState);
        _moveState.OnRequestExit += () => SetState(_idleState);
        PlayerHandPlaceState.OnPlaceCancel += () => SetState(_idleState);
    }
    private void OnDisable() {
        _idleState.OnRequestExit -= () => SetState(_moveState);
        _moveState.OnRequestExit -= () => SetState(_idleState);
        PlayerHandPlaceState.OnPlaceCancel -= () => SetState(_idleState);
    }


    protected override void Awake() {
        _input = GetComponent<PlayerInput>();
        _characterController = GetComponent<CharacterController>();

        base.Awake();
    }
    protected override void InitializeStates() {
        _idleState = new PlayerIdleState(this, _input);
        _moveState = new PlayerMoveState(this, _input);
        _stunState = new PlayerStunState(this, _input);
    }

    #endregion
}