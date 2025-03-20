using UnityEngine;

public sealed class PlayerHandEmptyState : AbstractPlayerHandState
{
    public PlayerHandEmptyState(PlayerHand Base) : base(Base) {
    }

    public override void Enter() {
        Base.SetCurrentItem<Item>(null);
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