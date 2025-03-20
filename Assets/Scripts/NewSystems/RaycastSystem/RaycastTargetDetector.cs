using UnityEngine;

namespace RedAxeGames
{
    public class RaycastTargetDetector : MonoBehaviour
    {
        [SerializeField] private LayerMask raycastTargetLayerMask = 0;
        [SerializeField] private LayerMask lineCastLayerMask = 0;

        [SerializeField] private float detectDistance = 2.00f;
        [SerializeField] private Transform origin = null;

        private RaycastTarget lastRaycastTarget = null;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(origin.position, origin.position + origin.forward * detectDistance);
        }

        private void Update()
        {
            DetectTarget();
        }

        private void DetectTarget()
        {
            if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, detectDistance, raycastTargetLayerMask))
            {
                if (hit.collider.TryGetComponent(out RaycastTarget target))
                {
                    if (Physics.Linecast(origin.position, target.transform.position, lineCastLayerMask))
                        return;

                    if (lastRaycastTarget is not null && !lastRaycastTarget.Equals(target))
                        lastRaycastTarget.OnMouseExit?.Invoke();

                    if (lastRaycastTarget is null || !lastRaycastTarget.Equals(target))
                        target.OnMouseEnter?.Invoke();

                    lastRaycastTarget = target;
                    return;
                }
            }

            if (lastRaycastTarget is not null)
            {
                lastRaycastTarget.OnMouseExit?.Invoke();
                lastRaycastTarget = null;
            }
        }
    }
}