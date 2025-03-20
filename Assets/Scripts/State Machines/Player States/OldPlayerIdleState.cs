using UnityEngine;

public class OldPlayerIdleState : OldState
{
    public OldPlayerIdleState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Player is now IDLE");
    }
}