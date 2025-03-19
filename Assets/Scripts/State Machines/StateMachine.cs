using System;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private State currentState;
    
    public State IdleState { get; private set; }
    public State WalkingState { get; private set; }
    public State RunningState { get; private set; }
    public State CrouchState { get; private set; }

    private void Awake()
    {
        IdleState = new PlayerIdleState(this);
        WalkingState = new PlayerWalkingState(this);
        RunningState = new PlayerRunState(this);
        CrouchState = new PlayerCrouchState(this);
        
        ChangeState(IdleState);
    }

    public void ChangeState(State newState)
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