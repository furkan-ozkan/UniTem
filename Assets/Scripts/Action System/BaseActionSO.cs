using System;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class BaseActionSO : ScriptableObject
{
    public static event Action<BaseActionSO, ActionContext> OnAnyActionExecuted;
    public static event Action<BaseActionSO, ActionContext> OnAnyActionUndone;

    public event Action<ActionContext> OnExecuted;
    public event Action<ActionContext> OnUndone;

    public async UniTask ExecuteAsync(ActionContext context, CancellationToken cancellationToken = default)
    {
        await OnExecute(context, cancellationToken);
        OnExecuted?.Invoke(context);
        OnAnyActionExecuted?.Invoke(this, context);
    }

    public async UniTask UndoAsync(ActionContext context, CancellationToken cancellationToken = default)
    {
        await OnUndo(context, cancellationToken);
        OnUndone?.Invoke(context);
        OnAnyActionUndone?.Invoke(this, context);
    }

    protected abstract UniTask OnExecute(ActionContext context, CancellationToken cancellationToken);
    protected virtual UniTask OnUndo(ActionContext context, CancellationToken cancellationToken) => UniTask.CompletedTask;
}


public class ActionContext
{
    public GameObject ActionPlayer { get; set; }
    public GameObject ActionItem { get; set; }
    public GameObject UsingItem { get; set; }
    public Dictionary<string, object> Parameters { get; private set; } = new Dictionary<string, object>();

    public T GetParameter<T>(string key, T defaultValue = default)
    {
        if (Parameters.TryGetValue(key, out object value) && value is T typedValue)
        {
            return typedValue;
        }
        return defaultValue;
    }

    public void SetParameter<T>(string key, T value)
    {
        Parameters[key] = value;
    }
}