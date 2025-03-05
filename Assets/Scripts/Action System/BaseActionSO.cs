using System;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;

public abstract class BaseActionSO : ScriptableObject, IActionExecutable
{
    public int ActionId;

    public event Action<ActionContext> OnExecuted;
    public event Action<ActionContext> OnUndone;

    public async UniTask ExecuteAsync(ActionContext context, CancellationToken cancellationToken = default)
    {
        await OnExecute(context, cancellationToken);
        OnExecuted?.Invoke(context);
    }

    public async UniTask UndoAsync(ActionContext context, CancellationToken cancellationToken = default)
    {
        await OnUndo(context, cancellationToken);
        OnUndone?.Invoke(context);
    }

    protected abstract UniTask OnExecute(ActionContext context, CancellationToken cancellationToken);
    protected virtual UniTask OnUndo(ActionContext context, CancellationToken cancellationToken) => UniTask.CompletedTask;

}