using UnityEngine;

public class PlayerLookLockedState : AbstractPlayerLookState
{
    public PlayerLookLockedState(PlayerLook Base) : base(Base) {
    }

    public override void Enter() {
        Base.PlayerCamera.transform.localRotation = Quaternion.Euler(Base.LastXRotation, 0.00f, 0.00f);
    }

    public override void Tick() {
    }

    public override void FixedTick() {
    }

    public override void LateTick() {
    }

    public override void Exit() {
    }
}