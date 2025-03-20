using UnityEngine;

public class RadioPuzzleState : OldState
{
    public RadioPuzzleState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered RADIO PUZZLE");
    }
}