using UnityEngine;

public class PlayerWalkingState : OldState
{
    public PlayerWalkingState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Player started WALKING");
    }
}