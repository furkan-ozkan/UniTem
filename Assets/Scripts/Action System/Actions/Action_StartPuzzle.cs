using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class Action_StartPuzzle : BaseAction<Context_Action_StartPuzzle>
{
    public override async UniTask ExecuteAsync(Context_Action_StartPuzzle context, CancellationToken cancellationToken = default)
    {
        MiniGameManager.StartMiniGame(context.miniGame, context.player);
        context.player.GetComponent<StateMachine>().ChangeState(MiniGameManager.GetState(context.miniGame));
    }

    public override async UniTask UndoAsync(Context_Action_StartPuzzle context, CancellationToken cancellationToken = default)
    {
        StateMachine playerStateMachine = context.player.GetComponent<StateMachine>();
        MiniGameManager.ResetMiniGame(context.miniGame);
        MiniGameManager.EndMiniGame(context.miniGame);
        playerStateMachine.ChangeState(playerStateMachine.IdleState);
    }
}