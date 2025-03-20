using UnityEngine;

public abstract class AbstractPlayerState<T> : State where T : MonoBehaviour
{
    protected T Base = default(T);

    protected Transform transform => Base.transform;

    public AbstractPlayerState(T Base) {
        this.Base = Base;
    }
}