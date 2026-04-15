using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class TestShooter : MonoBehaviour
    {
        public Transform target;

        [Header("References")]
        public GameObject bulletPrefab;

        [Header("Settings")]
        public float bulletSpeed = 20f;
        public float spawnOffset = 1.0f;

        private Camera mainCamera;
        private Data.ElementType type = Data.ElementType.Red;

        void Start()
        {
            mainCamera = Camera.main;

            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
        }

        void OnAttack(InputValue value)
        {
            if (value.isPressed)
            {
                Shoot();
            }
        }

        void OnQuickSlot1(InputValue value)
        {
            if (value.isPressed)
            {
                type = Data.ElementType.Red;
            }
        }

        void OnQuickSlot2(InputValue value)
        {
            if (value.isPressed)
            {
                type = Data.ElementType.Green;

            }
        }

        void OnQuickSlot3(InputValue value)
        {
            if (value.isPressed)
            {
                type = Data.ElementType.Blue;

            }
        }

        private void Shoot()
        {
            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnOffset;

            Vector3 direction = (target.localPosition - spawnPosition).normalized;

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}

