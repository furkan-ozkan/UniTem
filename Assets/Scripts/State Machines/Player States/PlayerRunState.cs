using UnityEngine;

public class PlayerRunState : OldState
{
    public PlayerRunState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Player started RUNNING");
    }
}