using UnityEngine;

public class PlayerCrouchState : OldState
{
    public PlayerCrouchState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Player started CROUCHING");
    }
}