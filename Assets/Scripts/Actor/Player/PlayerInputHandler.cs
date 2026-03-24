using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;

        private InputAction moveAction;
        private InputAction interactAction;
        private InputAction attackAction;
        private InputAction dashAction;
        private InputAction pointerAction;

        public bool isDashPressed { get; private set; }
        public Vector2 mouseInput { get; private set; }

        public float horizontal => moveAction.ReadValue<Vector2>().x;
        public float vertical => moveAction.ReadValue<Vector2>().y;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInput>();

            var actions = playerInput.actions;
            moveAction = actions["Move"];
            dashAction = actions["Dash"];
            pointerAction = actions["Pointer"];
            interactAction = actions["Interact"];
            attackAction = actions["Attack"];
            
        }

        private void OnEnable()
        {
            dashAction.started += OnDashStarted;
            dashAction.canceled += OnDashCanceled;

            pointerAction.performed += OnPointerPerformed;

            interactAction.performed += ctx => OnInteract();
        }

        private void OnDisable()
        {
            dashAction.started -= OnDashStarted;
            dashAction.canceled -= OnDashCanceled;

            pointerAction.performed -= OnPointerPerformed;
        }

        private void OnDashStarted(InputAction.CallbackContext context) => isDashPressed = true;
        private void OnDashCanceled(InputAction.CallbackContext context) => isDashPressed = false;

        private void OnPointerPerformed(InputAction.CallbackContext context) => mouseInput = context.ReadValue<Vector2>();

        private void OnInteract()
        {
            
        }
    }
}