using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class PlayerLookStateText : AbstractStateText<State>
{
    protected override string[] ReplaceStrings => _replaceStrings;
    private readonly string[] _replaceStrings = new string[] { "Player", "Look", "State" };

    protected override string TextPrefix => "Look State";
    protected override string TextColorName => "purple";


    private void OnEnable() {
        PlayerLook.OnChangedState += HandleOnChangedMoveState;
    }
    private void OnDisable() {
        PlayerLook.OnChangedState -= HandleOnChangedMoveState;
    }
}