using UnityEngine;

public sealed class PlayerMoveState : AbstractPlayerMovementState
{
    private float _currentSpeed = 0.00f;
    private Vector3 _moveDirection = Vector3.zero;

    private float _horizontal = 0.00f;
    private float _vertical = 0.00f;

    private bool IsSprint => PlayerInput.KeyPressing(PlayerInput.SprintKey);
    private bool _lastSprintValue = false;

    //public static event System.Action<bool> OnSprintStateChanged = null;


    public PlayerMoveState(PlayerMove PlayerMove, PlayerInput playerInput) : base(PlayerMove, playerInput) {
    }

    public override void Enter() {
    }

    public override void Tick() {
        _horizontal = PlayerInput.Horizontal;
        _vertical = PlayerInput.Vertical;

        if(_horizontal.Equals(0.00f) && _vertical.Equals(0.00f))
        {
            OnRequestExit?.Invoke();
            return;
        }    

        _currentSpeed = IsSprint ? Base.SprintSpeed : Base.MoveSpeed;
        _moveDirection = (transform.forward * (PlayerInput.Vertical) + transform.right * (PlayerInput.Horizontal)).normalized;

        Base.Controller.Move(_currentSpeed * Time.deltaTime * _moveDirection);

        if (IsSprint.Equals(_lastSprintValue))
            return;

        _lastSprintValue = IsSprint;
       // OnSprintStateChanged(IsSprint);
    }

    public override void FixedTick() {
    }
    public override void LateTick() {
    }
    public override void Exit() {
    }
}