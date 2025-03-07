using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;

public class ActionInvoker : MonoBehaviour
{
    // Artık BaseAction kullanıyoruz.
    private List<(BaseAction action, ActionContext context)> actionQueue = new List<(BaseAction, ActionContext)>();
    private (BaseAction action, ActionContext context)? lastAction = null;
    private CancellationTokenSource cancellationTokenSource;
    public bool isExecuting { get; private set; }
    private const int MaxQueueSize = 2;

    private async UniTask ProcessActionQueueAsync(CancellationToken cancellationToken = default)
    {
        isExecuting = true;
        cancellationTokenSource = new CancellationTokenSource();

        while (actionQueue.Count > 0)
        {
            var (action, context) = actionQueue[0];
            await ExecuteActionAsync(action, context, cancellationTokenSource.Token);
            actionQueue.RemoveAt(0);
        }
        
        isExecuting = false;
    }

    public void QueueAction(BaseAction action, ActionContext context)
    {
        if (action == null || context == null)
        {
            Debug.LogWarning("ActionInvoker: Cannot queue a null action or context.");
            return;
        }
        
        if(actionQueue.Count >= MaxQueueSize)
        {
            Debug.LogWarning("ActionInvoker: Action queue is full. Cannot add more actions.");
            return;
        }
        
        actionQueue.Add((action, context));

        if (!isExecuting)
        {
            ProcessActionQueueAsync().Forget();
        }
    }

    public void InsertActionAfterCurrent(BaseAction action, ActionContext context)
    {
        if (action == null || context == null)
        {
            Debug.LogWarning("ActionInvoker: Cannot insert a null action or context.");
            return;
        }

        if(actionQueue.Count >= MaxQueueSize)
        {
            Debug.LogWarning("ActionInvoker: Action queue is full. Cannot insert more actions.");
            return;
        }

        int insertIndex = (isExecuting && actionQueue.Count > 0) ? 1 : actionQueue.Count;
        actionQueue.Insert(insertIndex, (action, context));

        if (!isExecuting)
        {
            ProcessActionQueueAsync().Forget();
        }
    }

    public async UniTask ExecuteActionAsync(BaseAction action, ActionContext context, CancellationToken cancellationToken = default)
    {
        if (action == null || context == null)
        {
            Debug.LogWarning("ActionInvoker: Cannot execute a null action or context.");
            return;
        }

        await action.ExecuteAsync(context, cancellationToken);
        lastAction = (action, context);
    }

    public async UniTask UndoLastActionAsync(CancellationToken cancellationToken = default)
    {
        if (lastAction == null)
        {
            Debug.LogWarning("ActionInvoker: No action to undo.");
            return;
        }

        var (action, context) = lastAction.Value;
        await action.UndoAsync(context, cancellationToken);
        lastAction = null;
    }
}
