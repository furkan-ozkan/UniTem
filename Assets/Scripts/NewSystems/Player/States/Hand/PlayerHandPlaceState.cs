using RedAxeGames;
using UnityEngine;

public class PlayerHandPlaceState : AbstractPlayerHandState
{
    #region Private Variables

    private PlacePreviewItem _placePreviewItem = null;
    private PlayerLook _playerLook = null;
    private Vector3 _currentPlaceLocation = Vector3.zero;

    private float _mouseY = 0.00f;
    private float _rotateY = 0.00f;
    private readonly float _rotateSpeed = 100.00f;
    private bool _snap = false;

    #endregion

    public static event System.Action<bool> OnChangedSnap = null;

    public Item CurrentItem
    {
        get => _currentItem;
        set => _currentItem = value;
    }

    private Item _currentItem = default;

    private static readonly string PREVIEW_ITEM_PATH = "PlacePreviewItem";
    private static readonly string SNAP_CANVAS_PATH = "UI/PlaceSnapCanvas";

    private GameObject _snapCanvas = null;


    public static event System.Action OnPlaceEnd = null;
    public static event System.Action OnPlaceCancel = null;


    private PlayerInput _playerInput = null;

    public PlayerHandPlaceState(PlayerHand Base, PlayerInput Input, PlayerLook PlayerLook) : base(Base)
    {
        _playerInput = Input;
        _playerLook = PlayerLook;
    }

    public override void Enter()
    {
       // _placePreviewItem = Base.Spawn(Resources.Load<GameObject>(PREVIEW_ITEM_PATH), transform.position)
        //    .GetComponent<PlacePreviewItem>();

        _placePreviewItem.transform.SetParent(Base.transform.GetComponentInChildren<Camera>().transform);
      //  _snapCanvas = Base.Spawn(Resources.Load<GameObject>(SNAP_CANVAS_PATH));
        _placePreviewItem.Initialize(CurrentItem);

        CurrentItem.GetComponent<MeshRenderer>().enabled = false;
    }

    public override void Tick()
    {
        _mouseY += (Input.GetAxis("Mouse Y") * Time.deltaTime * 10.00f);
        _mouseY = Mathf.Clamp(_mouseY, 1.50f, 3.50f);

        if (Input.GetKeyDown(_playerInput.RotateSnapToggleKey))
        {
            _snap = !_snap;
            OnChangedSnap?.Invoke(_snap);

            if (_snap)
                _rotateY = 0.00f;
        }

        if (_snap)
        {
            if (Input.GetKeyDown(KeyCode.Q))
                _rotateY += 15.00f;
            if (Input.GetKeyDown(KeyCode.E))
                _rotateY -= 15.00f;
        }
        else
        {
            //if (Input.GetKey(KeyCode.Q))
            //    _rotateY += Time.deltaTime * _rotateSpeed * CurrentItem.RotateFactor;
          //  if (Input.GetKey(KeyCode.E))
              //  _rotateY -= Time.deltaTime * _rotateSpeed * CurrentItem.RotateFactor;
        }

        //
        // if (SendRay(out RaycastHit Hit))
        // {
        //     _placePreviewItem.IsHoldingHand = false;
        //     _placePreviewItem.transform.SetParent(null);
        //     _currentPlaceLocation = Hit.point + _currentItem.PlaceOverrideLocation;
        //
        //     _placePreviewItem.transform.position = Vector3.Lerp(
        //         _placePreviewItem.transform.position,
        //         _currentPlaceLocation,
        //         Base.PlacePreviewLerpSpeed * Time.deltaTime);
        //
        //     _placePreviewItem.transform.rotation = Quaternion.Euler(
        //         new Vector3(_placePreviewItem.transform.rotation.x, _rotateY, _placePreviewItem.transform.rotation.z) +
        //         _currentItem.PlaceOverrideRotation);
        // }
        // else
        // {
        //     _placePreviewItem.IsHoldingHand = true;
        //
        //     _placePreviewItem.transform.SetParent(Base.transform.GetComponentInChildren<Camera>().transform);
        //     _placePreviewItem.transform.localPosition = Vector3.Lerp(
        //         _placePreviewItem.transform.localPosition,
        //         _currentItem.GetComponent<Carryable>().HandOverrideLocation,
        //         Base.PlacePreviewLerpSpeed * 1.50f * Time.deltaTime);
        //
        //     _placePreviewItem.transform.localEulerAngles = _currentItem.GetComponent<Carryable>().HandOverrideRotation;
        // }

        if (Input.GetKeyUp(_playerInput.PlaceEndKey))
        {
         //   Base.Destroy(_placePreviewItem.gameObject);

            Vector3 rotation = new Vector3(_placePreviewItem.transform.rotation.x, _rotateY,
                _placePreviewItem.transform.rotation.z);

           // CurrentItem.Place(_currentPlaceLocation, rotation + _currentItem.PlaceOverrideRotation);

            OnPlaceEnd?.Invoke();
        }

        if (Input.GetKeyDown(_playerInput.PlaceCancelKey))
        {
         //   Base.Destroy(_placePreviewItem.gameObject);
            OnPlaceCancel?.Invoke();
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
        CurrentItem.GetComponent<MeshRenderer>().enabled = true;
      //  Base.Destroy(_snapCanvas);

        CurrentItem = null;
        _snap = false;
        _placePreviewItem = null;
        _mouseY = 0.00f;
        _rotateY = 0.00f;
    }


}