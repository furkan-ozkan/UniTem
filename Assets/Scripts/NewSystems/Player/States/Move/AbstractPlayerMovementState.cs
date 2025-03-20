using UnityEngine;

[System.Serializable]
public abstract class AbstractPlayerMovementState : AbstractPlayerState<PlayerMove>
{
    public System.Action OnRequestExit = null;

    protected PlayerInput PlayerInput = null;

    protected AbstractPlayerMovementState(PlayerMove Base, PlayerInput playerInput) : base(Base) {
        this.PlayerInput = playerInput;
    }
}