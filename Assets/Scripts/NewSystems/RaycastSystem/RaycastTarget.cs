using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

public class RaycastTarget : MonoBehaviour
{
    [SerializeField] private UnityEvent _onMouseEnter = null;
    [SerializeField] private UnityEvent _onMouseExit = null;

    [Button] private void InvokeOnMouseEnter() => _onMouseEnter?.Invoke();
    [Button] private void InvokeOnMouseExit() => _onMouseExit?.Invoke();
    
    public UnityEvent OnMouseEnter => _onMouseEnter;
    public UnityEvent OnMouseExit => _onMouseExit;
}