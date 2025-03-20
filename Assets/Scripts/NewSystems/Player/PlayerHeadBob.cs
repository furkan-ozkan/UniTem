using UnityEngine;
using Sirenix.OdinInspector;

[RequireComponent(typeof(PlayerLook))]
public class PlayerHeadBob : MonoBehaviour
{
    #region Serialized Variables

    [SerializeField, TabGroup("Enable")] private bool _enable = true;
    [SerializeField, Range(0.00f, 1.00f), TabGroup("Values")] private float _walkAmplitude = 0.015f;
    [SerializeField, Range(0.00f, 100.00f), TabGroup("Values")] private float _walkFrequency = 10.00f;

    [SerializeField, Range(0.00f, 2.00f), TabGroup("Values")] private float _sprintAmplitude = 0.015f;
    [SerializeField, Range(0.00f, 200.00f), TabGroup("Values")] private float _sprintFrequency = 10.00f;

    [SerializeField, TabGroup("References")] private Transform _camera = null;
    [SerializeField, TabGroup("References")] private Transform _cameraHolder = null;

    #endregion
    #region Private Variables

    private float _currentAmplitude = 0.00f;
    private float _currentFrequency = 0.00f;

    private Vector3 _startPos = Vector3.zero;
    private PlayerLook _playerLook = null;
    private bool _onComputer = false;

    #endregion
    #region Methods
    private void Awake() {
        _playerLook = GetComponent<PlayerLook>();
        _startPos = _camera.localPosition;
    }

    private void OnEnable() {
        //Computer.OnStartUse += HandleOnStartUseComputer;
        //Computer.OnStopUse += HandleOnStopUseComputer;

        //PlayerMoveState.OnSprintStateChanged += HandleOnChangedSprintState;
        PlayerMove.OnChangedState += (State) => _enable = State.GetType().Equals(typeof(PlayerMoveState));
    }
    private void OnDisable() {
        //Computer.OnStartUse -= HandleOnStartUseComputer;
        //Computer.OnStopUse -= HandleOnStopUseComputer;

        //PlayerMoveState.OnSprintStateChanged -= HandleOnChangedSprintState;
        PlayerMove.OnChangedState -= (State) => _enable = State.GetType().Equals(typeof(PlayerMoveState));
    }

    private void Start() {
        HandleOnChangedSprintState(false);
    }

    private void Update() {
        if (_enable)
            Logic();

        if (!_onComputer)
            ResetPosition();
    }

    private void Logic() {
        CheckMotion();

        if (_playerLook.IsCurrentState<PlayerLookFreeState>())
            _camera.LookAt(FocusTarget());
    }

    private Vector3 FootStepMotion() {
        Vector3 pos = Vector3.zero;
        pos.y += (Mathf.Sin(Time.time * _currentFrequency) * _currentAmplitude) * Time.deltaTime;
        pos.x += (Mathf.Cos(Time.time * _currentFrequency / 2) * _currentAmplitude * 2) * Time.deltaTime;
        return pos;
    }
    private void CheckMotion() {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        float speed = new Vector2(horizontal, vertical).magnitude;

        if (speed <= 0.00f) return;

        PlayMotion(FootStepMotion());
    }
    private void PlayMotion(Vector3 motion) {
        _camera.localPosition += motion;
    }

    private Vector3 FocusTarget() {
        Vector3 pos = new Vector3(transform.position.x, transform.position.y + _cameraHolder.localPosition.y, transform.position.z);
        pos += _cameraHolder.forward * 15.0f;
        return pos;
    }
    private void ResetPosition() {
        if (_camera.localPosition == _startPos) return;
        _camera.localPosition = Vector3.Lerp(_camera.localPosition, _startPos, 8 * Time.deltaTime);
    }

    private void HandleOnChangedSprintState(bool IsSprint) {
        _currentAmplitude = IsSprint ? _sprintAmplitude : _walkAmplitude;
        _currentFrequency = IsSprint ? _sprintFrequency : _walkFrequency;
    }

    private void HandleOnStartUseComputer() {
        _enable = false;
        _onComputer = true;
    }
    private void HandleOnStopUseComputer() {
        _enable = true;
        _onComputer = false;
    }

    #endregion
}