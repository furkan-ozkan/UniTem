using UnityEngine;

public abstract class ProviderBase<T> : ScriptableObject where T : Object
{
    public abstract bool Permission { get; }
}