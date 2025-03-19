using UnityEngine;

public class LockpickPuzzleState : State
{
    public LockpickPuzzleState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        base.Enter();
        Debug.Log("Entered LOCKPICK PUZZLE");
    }
}