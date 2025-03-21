using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class PlayerMoveStateText : AbstractStateText<State>
{
    protected override string[] ReplaceStrings { get => _replaceStrings; }

    private readonly string[] _replaceStrings = new string[] { "Player", "State" };

    protected override string TextPrefix => "Move State";
    protected override string TextColorName => "green";

    private void OnEnable() {
        PlayerMove.OnChangedState += HandleOnChangedMoveState;
    }
    private void OnDisable() {
        PlayerMove.OnChangedState -= HandleOnChangedMoveState;
    }
}