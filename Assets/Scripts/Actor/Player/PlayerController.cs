using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float rotationSpeed = 15f;

    private Rigidbody rb;
    private PlayerInputHandler inputHandler;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        inputHandler = GetComponent<PlayerInputHandler>();
    }

    private void FixedUpdate()
    {
        ApplyMovement();
    }

    private void ApplyMovement()
    {
        float currentSpeed = inputHandler.isDashPressed ? dashSpeed : walkSpeed;

        Vector3 moveDirection = new Vector3(inputHandler.moveInput.x, 0, inputHandler.moveInput.y);

        Vector3 targetVelocity = moveDirection * currentSpeed;
        rb.linearVelocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);

        if (moveDirection.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
        }
    }
}
