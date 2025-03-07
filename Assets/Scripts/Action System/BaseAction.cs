using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
public abstract class BaseAction
{
    public abstract UniTask ExecuteAsync(ActionContext context, CancellationToken cancellationToken = default);
    public abstract UniTask UndoAsync(ActionContext context, CancellationToken cancellationToken = default);
}

public abstract class BaseAction<TContext> : BaseAction where TContext : ActionContext
{
    public abstract UniTask ExecuteAsync(TContext context, CancellationToken cancellationToken = default);
    public abstract UniTask UndoAsync(TContext context, CancellationToken cancellationToken = default);

    public override async UniTask ExecuteAsync(ActionContext context, CancellationToken cancellationToken = default)
    {
        await ExecuteAsync((TContext)context, cancellationToken);
    }

    public override async UniTask UndoAsync(ActionContext context, CancellationToken cancellationToken = default)
    {
        await UndoAsync((TContext)context, cancellationToken);
    }
}
