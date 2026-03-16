using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 moveInput;
    private Rigidbody rb;
    public bool isDashPressed;

    

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnDash(InputValue value)
    {
        isDashPressed = value.isPressed;
    }
  
}