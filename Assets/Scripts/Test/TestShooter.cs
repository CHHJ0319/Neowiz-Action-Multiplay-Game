using UnityEngine;
using UnityEngine.InputSystem;

namespace Test
{
    public class TestShooter : MonoBehaviour
    {
        [Header("References")]
        public GameObject bulletPrefab;

        [Header("Settings")]
        public float bulletSpeed = 20f;
        public float spawnOffset = 1.0f;

        private Camera mainCamera;

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

        private void Shoot()
        {
            Ray ray = mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
            RaycastHit hit;
            Vector3 targetPoint;

            if (Physics.Raycast(ray, out hit))
            {
                targetPoint = hit.point;
            }
            else
            {
                targetPoint = ray.GetPoint(100f);
            }

            Vector3 spawnPosition = mainCamera.transform.position + mainCamera.transform.forward * spawnOffset;

            Vector3 direction = (targetPoint - spawnPosition).normalized;

            GameObject bullet = Instantiate(bulletPrefab, spawnPosition, Quaternion.LookRotation(direction));
            Rigidbody rb = bullet.GetComponent<Rigidbody>();

            if (rb != null)
            {
                rb.linearVelocity = direction * bulletSpeed;
            }
        }
    }
}

