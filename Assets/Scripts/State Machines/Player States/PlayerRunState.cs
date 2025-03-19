using UnityEngine;

public class PlayerRunState : State
{
    public PlayerRunState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Player started RUNNING");
    }
}