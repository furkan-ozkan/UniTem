using System;
using UnityEngine;

namespace RedAxeGames
{
    [RequireComponent(typeof(RaycastTarget))]
    public abstract class RaycastTargetListener : MonoBehaviour
    {
        protected RaycastTarget raycastTarget = null;

        protected virtual void Awake()
        {
            raycastTarget = GetComponent<RaycastTarget>();
        }

        protected virtual void OnEnable()
        {
            raycastTarget.OnMouseEnter.AddListener(OnRaycastTargetMouseEnter);
            raycastTarget.OnMouseExit.AddListener(OnRaycastTargetMouseExit);
        }

        protected virtual void OnDisable()
        {
            raycastTarget.OnMouseEnter.RemoveListener(OnRaycastTargetMouseEnter);
            raycastTarget.OnMouseExit.RemoveListener(OnRaycastTargetMouseExit);
        }

        protected virtual void OnRaycastTargetMouseEnter()
        {
        }

        protected virtual void OnRaycastTargetMouseExit()
        {
        }
    }
}