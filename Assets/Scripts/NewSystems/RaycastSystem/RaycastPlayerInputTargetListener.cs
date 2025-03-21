using UnityEngine;
using RedAxeGames;

[RequireComponent(typeof(RaycastPlayerInputTarget))]
public abstract class RaycastPlayerInputTargetListener : MonoBehaviour
{
    [SerializeField] protected bool autoUnregister = true;

    private RaycastPlayerInputTarget playerInputTarget = null;

    protected virtual void Awake()
    {
        playerInputTarget = GetComponent<RaycastPlayerInputTarget>();
        playerInputTarget.RegisterToPerformedEvent(OnPlayerInputTargetPerformed);
    }

    protected virtual void OnPlayerInputTargetPerformed()
    {
        if (autoUnregister)
            playerInputTarget.UnregisterToPerformedEvent(OnPlayerInputTargetPerformed);
    }
}