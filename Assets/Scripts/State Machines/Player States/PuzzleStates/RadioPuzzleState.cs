using UnityEngine;

public class RadioPuzzleState : State
{
    public RadioPuzzleState(StateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log("Entered RADIO PUZZLE");
    }
}