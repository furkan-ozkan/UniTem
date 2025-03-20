using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace RedAxeGames
{
    public class Placeable : MonoBehaviour
    {
        [SerializeField] private float placeMoveDuration = 0.25f, placeRotateDuration = 0.25f;
        [SerializeField] private Vector3 placeOverrideLocation = Vector3.zero;
        [SerializeField] private Vector3 placeOverrideRotation = Vector3.zero;

        [SerializeField] private UnityEvent onPlacedEvent = null;

        public Vector3 PlaceOverrideLocation => placeOverrideLocation;
        public Vector3 PlaceOverrideRotation => placeOverrideRotation;

        public void Place(Vector3 position, Vector3 rotation)
        {
            transform.SetParent(null);

            Sequence placeSequence = DOTween.Sequence();
            placeSequence.Append(transform.DOMove(position, placeMoveDuration));
            placeSequence.Join(transform.DORotate(rotation, placeRotateDuration));
            placeSequence.OnComplete(delegate { onPlacedEvent?.Invoke(); });
        }
    }
}