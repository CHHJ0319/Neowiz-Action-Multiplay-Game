using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;

        public Vector2 moveInput;
        public bool isDashPressed;
        public Vector2 mouseInput;
        public InputAction interactAction;
        public InputAction attackAction;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            interactAction = playerInput.actions["Interact"];
            attackAction = playerInput.actions["Attack"];
        }

        private void OnMove(InputValue value) => moveInput = value.Get<Vector2>();

        private void OnDash(InputValue value) => isDashPressed = value.isPressed;

        public void OnPointer(InputValue value)
        {
            mouseInput = value.Get<Vector2>();
        }
    }
}