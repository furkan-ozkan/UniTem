using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;

/// <summary>
/// New ActionInvoker: A dynamic and flexible action execution system.
/// Now continuously processes actions if available and properly handles undo logic.
/// Allows inserting actions immediately after the currently executing action.
/// </summary>
public class ActionInvoker : MonoBehaviour
{
    [Title("Action Queues")]
    [ShowInInspector, ReadOnly] 
    private List<(BaseActionSO action, ActionContext context)> queueView = new List<(BaseActionSO, ActionContext)>();

    [ShowInInspector, ReadOnly] 
    private List<(BaseActionSO action, ActionContext context)> historyView = new List<(BaseActionSO, ActionContext)>();

    private List<(BaseActionSO action, ActionContext context)> actionQueue = new List<(BaseActionSO, ActionContext)>();
    private Stack<(BaseActionSO action, ActionContext context)> actionHistory = new Stack<(BaseActionSO, ActionContext)>();
    private CancellationTokenSource cancellationTokenSource;
    private bool isExecuting = false;
    private int currentActionIndex = -1;

    /// <summary>
    /// Adds an action to the queue and ensures execution starts if not already running.
    /// </summary>
    public void QueueAction(BaseActionSO action, ActionContext context)
    {
        if (action == null || context == null)
        {
            Debug.LogWarning("ActionInvoker: Cannot queue a null action or context.");
            return;
        }
        
        actionQueue.Add((action, context));
        queueView.Add((action, context));

        if (!isExecuting)
        {
            ExecuteQueueAsync().Forget();
        }
    }

    /// <summary>
    /// Inserts an action immediately after the currently executing action.
    /// If no action is running, it is executed immediately.
    /// </summary>
    public void InsertActionAfterCurrent(BaseActionSO action, ActionContext context)
    {
        if (action == null || context == null)
        {
            Debug.LogWarning("ActionInvoker: Cannot insert a null action or context.");
            return;
        }

        int insertPosition = (currentActionIndex >= 0 && currentActionIndex < actionQueue.Count)
            ? currentActionIndex + 1
            : actionQueue.Count;

        actionQueue.Insert(insertPosition, (action, context));
        queueView.Insert(insertPosition, (action, context));

        if (!isExecuting)
        {
            ExecuteQueueAsync().Forget();
        }
    }

    /// <summary>
    /// Continuously executes actions from the queue until empty.
    /// </summary>
    private async UniTask ExecuteQueueAsync(CancellationToken cancellationToken = default)
    {
        isExecuting = true;
        cancellationTokenSource = new CancellationTokenSource();

        while (currentActionIndex + 1 < actionQueue.Count)
        {
            currentActionIndex++;
            var (action, context) = actionQueue[currentActionIndex];
            await ExecuteActionAsync(action, context, cancellationTokenSource.Token);
        }

        isExecuting = false;
        currentActionIndex = -1;
    }

    /// <summary>
    /// Executes a single action with its own unique context.
    /// </summary>
    public async UniTask ExecuteActionAsync(BaseActionSO action, ActionContext context, CancellationToken cancellationToken = default)
    {
        if (action == null || context == null)
        {
            Debug.LogWarning("ActionInvoker: Cannot execute a null action or context.");
            return;
        }

        await action.ExecuteAsync(context, cancellationToken);
        actionHistory.Push((action, context));
        historyView.Insert(0, (action, context));
    }

    /// <summary>
    /// Cancels all queued actions, allows the currently executing action to finish, and then undoes only completed actions.
    /// </summary>
    public async UniTask UndoAllActionsAsync(CancellationToken cancellationToken = default)
    {
        actionQueue.Clear();
        queueView.Clear();
        
        cancellationTokenSource?.Cancel();
        
        while (isExecuting)
        {
            await UniTask.Yield();
        }

        while (actionHistory.Count > 0)
        {
            await UndoLastActionAsync(cancellationToken);
        }
    }

    /// <summary>
    /// Undoes the last executed action if possible.
    /// </summary>
    public async UniTask UndoLastActionAsync(CancellationToken cancellationToken = default)
    {
        if (actionHistory.Count == 0)
        {
            Debug.LogWarning("ActionInvoker: No actions to undo.");
            return;
        }

        var (action, context) = actionHistory.Pop();
        historyView.RemoveAt(0);
        await action.UndoAsync(context, cancellationToken);
    }
}
