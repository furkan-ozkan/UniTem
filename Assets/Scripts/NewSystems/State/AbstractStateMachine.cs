using UnityEngine;

public abstract class AbstractStateMachine<T> : MonoBehaviour where T : State
{
    protected T CurrentState = default(T);
    protected abstract T InitializeState { get; }

    public static System.Action<T> OnChangedState = null;

    protected virtual void Awake() {
        InitializeStates();
        SetState(InitializeState);
    }

    protected void SetState(T NewState) {
        CurrentState?.Exit();
        CurrentState = NewState;

        OnChangedState?.Invoke(CurrentState);
        CurrentState?.Enter();
    }

    protected virtual void Update() {
        CurrentState?.Tick();
    }
    protected virtual void FixedUpdate() {
        CurrentState?.FixedTick();
    }
    protected virtual void LateUpdate() {
        CurrentState?.LateTick();
    }

    protected abstract void InitializeStates();

    public bool IsCurrentState<K>() where K : State => CurrentState.GetType() == typeof(K);
}