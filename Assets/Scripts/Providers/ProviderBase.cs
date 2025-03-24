using UnityEngine;

public abstract class ProviderBase<T> : ScriptableObject
{
    public abstract bool Permission { get; }
}