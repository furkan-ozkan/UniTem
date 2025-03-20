using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

namespace RedAxeGames
{
    public class Carryable : MonoBehaviour
    {
        [SerializeField] private Vector3 handOverrideLocation = Vector3.zero;
        [SerializeField] private Vector3 handOverrideRotation = Vector3.zero;
        [SerializeField] private int carryingLayerIndex = 0;

        [SerializeField] private bool calculatePerFrame = false;

        [SerializeField] private UnityEvent onCarriedEvent = new UnityEvent();

        public UnityEvent OnCarriedEvent => onCarriedEvent;
        public Vector3 HandOverrideLocation => handOverrideLocation;
        public Vector3 HandOverrideRotation => handOverrideRotation;


        private Rigidbody rigidBody = null;
        private Collider collider = null;
        private Transform targetParent = null;
        private int defaultLayer = 0;

        protected virtual void Awake()
        {
            rigidBody = GetComponent<Rigidbody>();
            collider = GetComponent<Collider>();
            defaultLayer = gameObject.layer;
        }

        private void Start()
        {
            targetParent = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Camera>().transform;
        }

        private void Update()
        {
            if (calculatePerFrame && transform.parent.Equals(targetParent))
            {
                transform.localPosition = HandOverrideLocation;
                transform.localRotation = Quaternion.Euler(HandOverrideRotation);
            }
        }

        public void Carry()
        {
            gameObject.layer = carryingLayerIndex;
            collider.isTrigger = true;
            rigidBody.isKinematic = true;

            transform.SetParent(targetParent);
            transform.DOKill();

            Sequence carrySequence = DOTween.Sequence();
            carrySequence.Append(transform.DOLocalMove(HandOverrideLocation, 0.40f).SetEase(Ease.OutBack));
            carrySequence.Join(transform.DOLocalRotate(HandOverrideRotation, 0.40f));
            carrySequence.OnComplete(delegate { OnCarriedEvent?.Invoke(); });
        }

        public void SetLayerToDefault()
        {
            gameObject.layer = defaultLayer;
        }
    }
}