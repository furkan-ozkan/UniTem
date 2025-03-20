using UnityEngine;

public sealed class PlayerIdleState : AbstractPlayerMovementState
{
    public PlayerIdleState(PlayerMove PlayerMove, PlayerInput playerInput) : base(PlayerMove, playerInput) {
    }
    public override void Enter() {
        Base.Controller.Move(Vector3.zero);
    }
    public override void Tick() {
        if (PlayerInput.Horizontal.Equals(0.00f) && PlayerInput.Vertical.Equals(0.00f))
            return;

        OnRequestExit?.Invoke();
    }


    public override void FixedTick() {
    }
    public override void LateTick() {
    }
    public override void Exit() {
    }
}