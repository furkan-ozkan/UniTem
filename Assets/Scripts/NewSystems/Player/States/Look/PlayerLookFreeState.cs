using UnityEngine;

public class PlayerLookFreeState : AbstractPlayerLookState
{
    private float _mouseX = 0.00f;
    private float _mouseY = 0.00f;

    private float _yRotation = 0.00f;
    private float _xRotation = 0.00f;

    public PlayerLookFreeState(PlayerLook Base) : base(Base) {
    }

    public override void Enter() {
    }

    public override void Tick() {
        _mouseX = Input.GetAxis("Mouse X")* Base.Sensitivity.x; // * Time.deltaTime 
        _mouseY = Input.GetAxis("Mouse Y")* Base.Sensitivity.y; // * Time.deltaTime 

        _xRotation -= _mouseY;
        _yRotation = _mouseX;


        _xRotation = Mathf.Clamp(_xRotation, Base.LookBorders.x, Base.LookBorders.y);

        Base.PlayerCamera.transform.localRotation = Quaternion.Euler(_xRotation, 0.00f, 0.00f);
        transform.rotation *= Quaternion.Euler(0.00f, _yRotation, 0.00f);
    }

    public override void FixedTick() {
    }

    public override void LateTick() {
    }

    public override void Exit() {
        Base.LastXRotation = _xRotation;
    }
}