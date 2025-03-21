using UnityEngine;

public enum PuzzleType
{
    None,
    Lockpick,
    Radio
}

public class PlayerPuzzleState : OldState
{
    private StateMachine puzzleStateMachine;
    private PuzzleType puzzleType;

    public PlayerPuzzleState(StateMachine stateMachine, PuzzleType puzzleType) : base(stateMachine)
    {
        puzzleStateMachine = new StateMachine();
        this.puzzleType = puzzleType;
    }

    public override void Enter()
    {
        Debug.Log($"Player entered {puzzleType} PUZZLE");
        // stateMachine.GetComponent<InventorySlotContainer>().PutSelectedItemInInventory();
        stateMachine.GetComponent<Interaction>().enabled = false;
        // stateMachine.GetComponent<InventorySlotContainer>().enabled = false;

        switch (puzzleType)
        {
            case PuzzleType.Lockpick:
                puzzleStateMachine.ChangeState(new LockpickPuzzleState(puzzleStateMachine));
                break;
            case PuzzleType.Radio:
                puzzleStateMachine.ChangeState(new RadioPuzzleState(puzzleStateMachine));
                break;
        }
    }

    public override void Update()
    {
        puzzleStateMachine.Update();
    }

    public override void Exit()
    {
        Debug.Log("Player exited PUZZLE MODE");
        stateMachine.GetComponent<Interaction>().enabled = true;
        // stateMachine.GetComponent<InventorySlotContainer>().enabled = true;
    }
}