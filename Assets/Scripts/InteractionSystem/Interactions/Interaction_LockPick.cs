using System;
using UnityEngine;

public class Interaction_LockPick : BaseInteractable
{
    [SerializeField] private MiniGame _lockPickMiniGame;
    public override bool Interact(GameObject player)
    {
        base.Interact(player);
        Context_Action_StartPuzzle context = new Context_Action_StartPuzzle(player, _lockPickMiniGame);
        _actionInvoker.QueueAction(new Action_StartPuzzle(),context);
        return true;
    }

    public override void StartHover()
    {
        base.StartHover();
    }

    public override void EndHover()
    {
        base.EndHover();
    }
}