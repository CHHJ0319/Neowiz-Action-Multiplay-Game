using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player
{
    public class PlayerInputHandler : MonoBehaviour
    {
        private PlayerInput playerInput;

        private InputAction moveAction;
        public InputAction dashAction;
        private InputAction pointerAction;
        public InputAction attackAction;
        public InputAction interactAction;
        public InputAction quickSlot1Action;

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
            attackAction = actions["Attack"];
            interactAction = actions["Interact"];
            quickSlot1Action = actions["QuickSlot1"];
        }

        private void OnEnable()
        {
            dashAction.started += OnDashStarted;
            dashAction.canceled += OnDashCanceled;

            pointerAction.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        }

        private void OnDisable()
        {
            dashAction.started -= OnDashStarted;
            dashAction.canceled -= OnDashCanceled;

        }

        public void SetPlayerInputEnabled(bool isEnabled)
        {
            playerInput.enabled = isEnabled;

            if (isEnabled)
            {
                playerInput.actions.Enable();
            }
            else
            {
                playerInput.actions.Disable();
            }
        }

        private void OnDashStarted(InputAction.CallbackContext context) => isDashPressed = true;
        private void OnDashCanceled(InputAction.CallbackContext context) => isDashPressed = false;
    }
}