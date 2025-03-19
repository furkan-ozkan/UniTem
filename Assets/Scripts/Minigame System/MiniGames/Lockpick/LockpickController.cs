using System;
using UnityEngine;

public class LockpickController : MonoBehaviour
{
    [SerializeField] private float leftBound;
    [SerializeField] private float rightBound;
    private void OnEnable()
    {
        InputProvider.OnLookInput += MoveLockpick;
    }

    private void OnDisable()
    {
        InputProvider.OnLookInput -= MoveLockpick;
    }

    void Update()
    {
        
    }
    
    private void MoveLockpick(Vector2 mousePosition)
    {
        float t = Mathf.InverseLerp(0, Screen.width, mousePosition.x);
        float newPositionX = Mathf.Lerp(leftBound, rightBound, t);
        Vector3 newPosition = new Vector3(newPositionX, transform.localPosition.y, transform.localPosition.z);
        transform.localPosition = newPosition;
    }
}