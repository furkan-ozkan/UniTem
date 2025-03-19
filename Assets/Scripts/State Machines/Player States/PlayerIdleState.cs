using UnityEngine;

public class PlayerIdleState : State
{
    public PlayerIdleState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Player is now IDLE");
    }
}