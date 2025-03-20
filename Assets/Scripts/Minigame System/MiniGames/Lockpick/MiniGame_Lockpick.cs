using UnityEngine;

public class MiniGame_Lockpick : MiniGame
{
    [SerializeField] private GameObject miniGameCamera;
    
    [SerializeField] private LockpickController lockpickController;
    
    public override void Initialize(GameObject player)
    {
        AddInputProviders();
        this.player = player;
        miniGameCamera.SetActive(true);
        lockpickController.enabled = true;
    }

    public override void EndMiniGame()
    {
        RemoveInputProviders();
        EventManager.RecordEvent("LockpickEnd");
        miniGameCamera.SetActive(false);
        lockpickController.enabled = false;
        player.GetComponent<StateMachine>().ChangeState(new OldPlayerIdleState(player.GetComponent<StateMachine>()));
        player = null;
    }

    public override void CompleteMiniGame()
    {
        
    }

    public override void ResetMiniGame()
    {
        
    }

    public override OldState GetState()
    {
        return new PlayerPuzzleState(player.GetComponent<StateMachine>(), PuzzleType.Lockpick);
    }
}