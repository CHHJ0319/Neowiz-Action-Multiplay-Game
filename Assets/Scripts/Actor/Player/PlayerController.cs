using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float dashSpeed = 10f;
    [SerializeField] private float rotationSpeed = 15f;

    [Header("Attack Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float bulletSpeed = 20f;

    [Header("Pointer Settings")]
    [SerializeField] private Transform pointer;
    [SerializeField] private float pointerSpeed = 10f;
    [SerializeField] private float distanceFromCamera = 10f;


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
        Shoot();
        MovePointer();
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

    private void Shoot()
    {
        if (inputHandler.isFirePressed)
        {
            GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

            Vector3 direction = (pointer.position - firePoint.position).normalized;
            direction.y = 0;
            direction = direction.normalized;

            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
            if (bulletRb != null)
            {
                bulletRb.linearVelocity = direction * bulletSpeed;
            }

            bullet.transform.forward = direction;
        }
    }

    private void MovePointer()
    {
        Vector3 screenPosition = new Vector3(inputHandler.mouseInput.x, inputHandler.mouseInput.y, distanceFromCamera);

        Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

        pointer.position = Vector3.Lerp(pointer.position, targetPosition, pointerSpeed * Time.deltaTime);
    }
}
