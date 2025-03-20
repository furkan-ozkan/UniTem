using UnityEngine;
using Sirenix.OdinInspector;

public sealed class PlayerLook : AbstractStateMachine<AbstractPlayerLookState>
{
    #region Serialized Variables

    [SerializeField, FoldoutGroup("Look Variables")] private Vector2 _sensitivity = Vector2.one;
    [SerializeField, MinMaxSlider(-90.00f, 90.00f), FoldoutGroup("Look Variables")] private Vector2Int _lookBorders = Vector2Int.one;

    [SerializeField, FoldoutGroup("Mouse Misc")] private bool _mouseLock = true;
    [SerializeField, FoldoutGroup("Mouse Misc")] private bool _invisibleMouse = true;

    #endregion
    #region Private Variables

    private Camera _playerCamera = null;

    private PlayerLookLockedState _lockedState = null;
    private PlayerLookFreeState _freeState = null;
    #endregion
    #region Encapsulations
    public float LastXRotation { get; set; } = 0.00f;

    public Vector2 Sensitivity => _sensitivity * 100.00f;
    public Vector2 LookBorders => _lookBorders;
    public Camera PlayerCamera => _playerCamera;

    protected override AbstractPlayerLookState InitializeState => _freeState;

    #endregion
    #region Methods

    protected override void Awake() {
        base.Awake();
        _playerCamera = GetComponentInChildren<Camera>();
        _playerCamera ??= FindObjectOfType<Camera>();
    }

    private void OnEnable() {
        //Computer.OnStartUse += () => SetState(_lockedState);
        //Computer.OnStopUse += () => SetState(_freeState);

        //PressMachine.OnUsedEvent += () => SetState(_lockedState);
       // PressMachine.OnExitEvent += () => SetState(_freeState);

        //PlayerHandCarryState.OnStartPlace += (OldItem) => SetState(_lockedState);
        //PlayerHandPlaceState.OnPlaceEnd += () => SetState(_freeState);
        //PlayerHandPlaceState.OnPlaceCancel += () => SetState(_freeState);

    }
    private void OnDisable() {
        //Computer.OnStartUse -= () => SetState(_lockedState);
       // Computer.OnStopUse -= () => SetState(_freeState);

       // PlayerHandCarryState.OnStartPlace -= (OldItem) => SetState(_lockedState);
       // PlayerHandPlaceState.OnPlaceEnd -= () => SetState(_freeState);
       // PlayerHandPlaceState.OnPlaceCancel -= () => SetState(_freeState);
    }

    private void Start() {
        Cursor.lockState = _mouseLock ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !_invisibleMouse;
    }

    protected override void Update() {
        if (Input.GetKeyDown(KeyCode.K))
            SetState(_lockedState);
        if (Input.GetKeyDown(KeyCode.J))
            SetState(_freeState);

        base.Update();
    }

    protected override void InitializeStates() {
        _freeState = new PlayerLookFreeState(this);
        _lockedState = new PlayerLookLockedState(this);
    }

    #endregion
}