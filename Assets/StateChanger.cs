// TODO => For testing, this script will be removed later
// TODO => For testing, this script will be removed later
// TODO => For testing, this script will be removed later

using UnityEngine;

public class StateChanger : MonoBehaviour
{
    public StateMachine stateMachine;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            stateMachine.ChangeState(new PlayerPuzzleState(stateMachine, PuzzleType.Radio)); 
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            stateMachine.ChangeState(new PlayerPuzzleState(stateMachine, PuzzleType.Lockpick));
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            stateMachine.ChangeState(stateMachine.IdleState);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            stateMachine.ChangeState(stateMachine.RunningState);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            stateMachine.ChangeState(stateMachine.WalkingState);
        }
    }
}