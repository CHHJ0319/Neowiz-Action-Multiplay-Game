using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    public Vector2 moveInput; 

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
}