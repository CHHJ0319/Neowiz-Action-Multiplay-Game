using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 moveInput;
    public bool isDashPressed;
    private bool _isFirePressed;
    public bool isFirePressed
    {
        get
        {
            if (_isFirePressed)
            {
                _isFirePressed = false; 
                return true;           
            }
            return false;
        }
    }
    public Vector2 mouseInput;

    private void Awake()
    {

    }

    private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

    private void OnDash(InputValue value) => isDashPressed = value.isPressed;

    private void OnFire(InputValue value)
    {
        if (value.isPressed)
        {
            _isFirePressed = true;
        }
    }

    public void OnPointer(InputValue value)
    {
        mouseInput = value.Get<Vector2>();
    }
}