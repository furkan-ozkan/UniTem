using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using Sirenix.OdinInspector;

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


    #region Queue Based Actions

    /// <summary>
    /// Continuously processes queued actions until empty.
    /// </summary>
    private async UniTask ProcessActionQueueAsync(CancellationToken cancellationToken = default)
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
    
    #region Add To Queue

    public void AddToQueue(BaseActionSO action, ActionContext context)
    {
        RPC_AddToQueue(action.ActionId, ActionContextUtility.ConvertContextToJson(context));
    }
    
    private void RPC_AddToQueue(int actionID, string context)
    {
        BaseActionSO actionSo = ActionDatabase.Instance.GetActionById(actionID);
        if (actionSo == null)
        {
            Debug.LogWarning($"ActionInvoker: Action with ID {actionID} not found.");
            return;
        }

        ActionContext actionContext = ActionContextUtility.ConvertJsonToContext(context);
        QueueAction(actionSo, actionContext);
    }
    
    /// <summary>
    /// Adds an action to the queue and starts processing if not already running.
    /// </summary>
    private void QueueAction(BaseActionSO action, ActionContext context)
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
            ProcessActionQueueAsync().Forget();
        }
    }

    #endregion

    #region Insert Action After Current

    public void AddActionAfterCurrent(BaseActionSO action, ActionContext context)
    {
        RPC_AddActionAfterCurrent(action.ActionId, ActionContextUtility.ConvertContextToJson(context));
    }
    
    private void RPC_AddActionAfterCurrent(int actionID, string context)
    {
        BaseActionSO actionSo = ActionDatabase.Instance.GetActionById(actionID);
        if (actionSo == null)
        {
            Debug.LogWarning($"ActionInvoker: Action with ID {actionID} not found.");
            return;
        }

        ActionContext actionContext = ActionContextUtility.ConvertJsonToContext(context);
        
        InsertActionAfterCurrent(actionSo, actionContext);
    }
    
    /// <summary>
    /// Inserts an action immediately after the currently executing action.
    /// </summary>
    private void InsertActionAfterCurrent(BaseActionSO action, ActionContext context)
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
            ProcessActionQueueAsync().Forget();
        }
    }

#endregion

    #endregion

    #region Execute Single Action

    public void ExecuteAction(BaseActionSO action, ActionContext context)
    {
        RPC_ExecuteAction(action.ActionId, ActionContextUtility.ConvertContextToJson(context));
    }
    
    private void RPC_ExecuteAction(int actionID, string context)
    {
        BaseActionSO actionSo = ActionDatabase.Instance.GetActionById(actionID);
        if (actionSo == null)
        {
            Debug.LogWarning($"ActionInvoker: Action with ID {actionID} not found.");
            return;
        }

        ActionContext actionContext = ActionContextUtility.ConvertJsonToContext(context);
        
        ExecuteActionAsync(actionSo, actionContext);
    }
    
    /// <summary>
    /// Executes a single action and stores it in history.
    /// </summary>
    private async UniTask ExecuteActionAsync(BaseActionSO action, ActionContext context, CancellationToken cancellationToken = default)
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

    #endregion

    #region Undo Actions

    /// <summary>
    /// Cancels all queued actions and undoes completed ones.
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
    /// Undoes the last executed action.
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

    #endregion
}
