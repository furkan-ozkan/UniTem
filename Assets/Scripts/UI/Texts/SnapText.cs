using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public sealed class SnapText : MonoBehaviour
{
    private TextMeshProUGUI _text = null;
    private void Awake() {
        _text = GetComponent<TextMeshProUGUI>();
    }
    
    private void OnEnable() {
        PlayerHandPlaceState.OnChangedSnap += HandleOnChangedSnap;
    }
    private void OnDisable() {
        PlayerHandPlaceState.OnChangedSnap -= HandleOnChangedSnap;
    }

    private void HandleOnChangedSnap(bool Snap) {
        _text.color = Snap ? Color.green : Color.red;
    }
}