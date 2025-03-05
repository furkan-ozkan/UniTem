using System.Threading;
using Cysharp.Threading.Tasks;

public interface IActionExecutable
{
    UniTask ExecuteAsync(ActionContext context, CancellationToken cancellationToken = default);
    UniTask UndoAsync(ActionContext context, CancellationToken cancellationToken = default);
}