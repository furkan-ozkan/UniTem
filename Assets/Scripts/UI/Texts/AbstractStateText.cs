using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public abstract class AbstractStateText<T> : MonoBehaviour where T : State
{
    protected TextMeshProUGUI StateText = null;

    protected abstract string[] ReplaceStrings { get; }

    protected abstract string TextPrefix { get; }
    protected abstract string TextColorName { get; }

    private void Awake() {
        StateText = GetComponent<TextMeshProUGUI>();
    }

    protected virtual void HandleOnChangedMoveState(T NewState) {
        StateText.SetText($"{TextPrefix}: <color={TextColorName}>{GetReplacedString(NewState.ToString())}</color>");
    }
    protected string GetReplacedString(string NonReplacedString) {
        foreach (string replaceValue in ReplaceStrings)
            NonReplacedString = NonReplacedString.Replace(replaceValue, string.Empty);

        return NonReplacedString;
    }
}