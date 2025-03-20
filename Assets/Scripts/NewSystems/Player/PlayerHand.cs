using UnityEngine;

[RequireComponent(typeof(PlayerLook), typeof(PlayerInput))]
public sealed class PlayerHand : AbstractStateMachine<AbstractPlayerHandState>
{
    [SerializeField] private LayerMask placableLayerMask = 0;
    [SerializeField] private float placeRayDistance = 3.00f;
    [SerializeField] private float placePreviewLerpSpeed = 6.00f;
    [SerializeField] private float placePreviewRotateDegree = 15.00f;
    
    public LayerMask PlacableLayerMask => placableLayerMask;
    public float RotateDegree => placePreviewRotateDegree;

    public float PlacePreviewLerpSpeed => placePreviewLerpSpeed;
    public float PlaceRayDistance => placeRayDistance;
    
    private PlayerInput _playerInput = null;

    private PlayerHandEmptyState _emptyState = null;
    private PlayerHandCarryState _carryState = null;

    protected override AbstractPlayerHandState InitializeState => _emptyState;

    protected override void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        base.Awake();
    }

    protected override void InitializeStates()
    {
        _emptyState = new PlayerHandEmptyState(this);
        _carryState = new PlayerHandCarryState(this, _playerInput, GetComponent<PlayerLook>());
    }
    
    public void SetCurrentItem<T>(T newValue) where T : Item => _carryState.CurrentItem = newValue;

    public void CarriedItem<T>(T item) where T : Item
    {
        SetCurrentItem(item);
        SetState(_carryState);
    }

    public void PlaceItem<T>(T item, Vector3 location, Vector3 rotation) where T : Item
    {
        item.LocalPlaceable.Place(location, rotation);
        SetState(_emptyState);
    }
}