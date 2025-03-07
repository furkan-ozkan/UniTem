using System.Threading;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using UnityEngine;

public class Action_Fire : BaseAction<Context_Action_Fire>
{
    public override async UniTask ExecuteAsync(Context_Action_Fire context, CancellationToken cancellationToken = default)
    {
        GameObject spawnedBullet = context.bulletPrefab;
        spawnedBullet.transform.position = context.spawnPosition;
        spawnedBullet.transform.LookAt(context.direction);
        Vector3 targetPosition = context.spawnPosition + context.direction * context.range;
        
        var tcs = new UniTaskCompletionSource();
        spawnedBullet.SetActive(true);
        spawnedBullet.transform.DOMove(targetPosition, context.speed).OnComplete(() =>
        {
            spawnedBullet.SetActive(false);
            spawnedBullet.transform.position = context.spawnPosition;
            tcs.TrySetResult();
        });
        await tcs.Task;
    }

    public override UniTask UndoAsync(Context_Action_Fire context, CancellationToken cancellationToken = default)
    {
        throw new System.NotImplementedException();
    }
}