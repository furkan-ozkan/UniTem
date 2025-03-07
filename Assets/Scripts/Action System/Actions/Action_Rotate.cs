
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;
using UnityEngine;

public class Action_Rotate : BaseAction<Context_Action_Rotate>
{
    private Vector3 _rotationAngle; 
    private float _rotationTime; 
    public override async UniTask ExecuteAsync(Context_Action_Rotate context, CancellationToken cancellationToken = default)
    {
        Transform rotateObject = context.rotateObject.transform;
        
        Vector3 rotationAngle = context.rotationAngle;
        float rotationTime = context.rotationTime;
        
        Vector3 finalRotation = rotationAngle;
        
        if (context.axis != Context_Action_Rotate_Axis.all)
        {
            Vector3 currentRotation = rotateObject.eulerAngles;
            switch(context.axis)
            {
                case Context_Action_Rotate_Axis.x:
                    finalRotation = new Vector3(rotationAngle.x, currentRotation.y, currentRotation.z);
                    break;
                case Context_Action_Rotate_Axis.y:
                    finalRotation = new Vector3(currentRotation.x, rotationAngle.y, currentRotation.z);
                    break;
                case Context_Action_Rotate_Axis.z:
                    finalRotation = new Vector3(currentRotation.x, currentRotation.y, rotationAngle.z);
                    break;
                default:
                    finalRotation = rotationAngle;
                    break;
            }
        }
        var tcs = new UniTaskCompletionSource();
        rotateObject.DORotate(finalRotation, rotationTime).OnComplete(() => tcs.TrySetResult());
        await tcs.Task;
        await UniTask.Yield();
    }

    public override async UniTask UndoAsync(Context_Action_Rotate context, CancellationToken cancellationToken = default)
    {
        Debug.Log("Undoing Rotate Action");
        await UniTask.Yield();
    }
}