public sealed class PlayerHandStateText : AbstractStateText<State>
{
    protected override string[] ReplaceStrings => _replaceStrings;
    private readonly string[] _replaceStrings = new string[] { "Player", "Hand", "State" };

    protected override string TextPrefix => "Hand State";
    protected override string TextColorName => "grey";

    private void OnEnable() {
        PlayerHand.OnChangedState += HandleOnChangedMoveState;
    }
    private void OnDisable() {
        PlayerHand.OnChangedState -= HandleOnChangedMoveState;
    }
}