using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private OldState currentState;
    
    public OldState IdleState { get; private set; }
    public OldState WalkingState { get; private set; }
    public OldState RunningState { get; private set; }
    public OldState CrouchState { get; private set; }

    private void Awake()
    {
        IdleState = new OldPlayerIdleState(this);
        WalkingState = new PlayerWalkingState(this);
        RunningState = new PlayerRunState(this);
        CrouchState = new PlayerCrouchState(this);
        
        ChangeState(IdleState);
    }

    public void ChangeState(OldState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void Update()
    {
        currentState?.Update();
    }
}