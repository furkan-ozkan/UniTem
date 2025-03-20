using RedAxeGames;
using UnityEngine;

public sealed class PlayerHandCarryState : AbstractPlayerHandState
{
    public Item CurrentItem { get; set; } = default;


    private readonly float _timeDurationToCanStartToPlace = 0.50f;
    private float _elapsedTime = 0.00f;
    private bool _canStartToPlace = false;

    private PlacePreviewItem placePreviewItem = null;
    private Vector3 currentPlaceLocation = Vector3.zero;

    private static readonly string PREVIEW_ITEM_PATH = "PlacePreviewItem";
    private float rotateY = 0.00f;

    private readonly PlayerLook playerLook = null;
    private readonly PlayerInput playerInput = null;

    public PlayerHandCarryState(PlayerHand Base, PlayerInput Input, PlayerLook PlayerLook) : base(Base)
    {
        playerInput = Input;
        playerLook = PlayerLook;
    }

    public override void Enter()
    {
        placePreviewItem = Object.Instantiate(Resources.Load<GameObject>(PREVIEW_ITEM_PATH), transform.position, Quaternion.identity)
            .GetComponent<PlacePreviewItem>();

        placePreviewItem.transform.SetParent(Base.transform.GetComponentInChildren<Camera>().transform);
        placePreviewItem.Initialize(CurrentItem);
        placePreviewItem.SetVisibleState(false);
    }

    public override void Tick()
    {
        _elapsedTime += Time.deltaTime;
        _canStartToPlace = _elapsedTime >= _timeDurationToCanStartToPlace;

        if (_canStartToPlace)
        {
            Debug.DrawRay(GetRay().origin, GetRay().direction, Color.red);
            if (SendRay(out RaycastHit Hit))
            {
                placePreviewItem.SetVisibleState(true);

                rotateY += (Input.mouseScrollDelta.y != 0.00f ? Input.mouseScrollDelta.y : 0.00f) * Base.RotateDegree;

                placePreviewItem.transform.SetParent(null);
                currentPlaceLocation = Hit.point + CurrentItem.LocalPlaceable.PlaceOverrideLocation;

                placePreviewItem.transform.position = Vector3.Lerp(placePreviewItem.transform.position, currentPlaceLocation, Base.PlacePreviewLerpSpeed * Time.deltaTime);
                placePreviewItem.transform.rotation = Quaternion.Euler(new Vector3(placePreviewItem.transform.rotation.x, rotateY, placePreviewItem.transform.rotation.z) + CurrentItem.LocalPlaceable.PlaceOverrideRotation);
            }
            else
            {
                placePreviewItem.SetVisibleState(false);
                rotateY = 0.00f;
            }

            if (Input.GetKeyUp(KeyCode.Mouse0) && placePreviewItem.CanPlace())
            {
                Object.Destroy(placePreviewItem.gameObject);
                Vector3 rotation = new Vector3(placePreviewItem.transform.rotation.x, rotateY,
                    placePreviewItem.transform.rotation.z);

                Base.PlaceItem(CurrentItem, currentPlaceLocation, rotation + CurrentItem.LocalPlaceable.PlaceOverrideRotation);
            }
        }
    }

    public override void FixedTick()
    {
    }

    public override void LateTick()
    {
    }

    public override void Exit()
    {
        _elapsedTime = 0.00f;
        rotateY = 0.00f;
        _canStartToPlace = false;
    }


    private Ray GetRay()
    {
        Transform CameraTransform = playerLook.PlayerCamera.transform;

        return new Ray(CameraTransform.position, CameraTransform.forward);
    }

    private bool SendRay(out RaycastHit Hit)
    {
        return Physics.Raycast(GetRay(), out Hit, Base.PlaceRayDistance, Base.PlacableLayerMask);
    }
}