using Actor.Weapon;
using UnityEngine;

namespace Actor.Player 
{
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

        [SerializeField] private Transform itemHolder;

        private Rigidbody rb;
        private PlayerInputHandler inputHandler;

        public Data.PlayerType PlayerType = Data.PlayerType.Shooter;
        public int ammo;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputHandler = GetComponent<PlayerInputHandler>();

            ammo = 10;
        }

        private void FixedUpdate()
        {
            ApplyMovement();
            Shoot();
            MovePointer();
        }

        private void OnTriggerStay(Collider other)
        {

            if (inputHandler.interactAction.triggered)
            {
                if (other.CompareTag("Item"))
                {
                    PickUp(other.gameObject);
                }
            }
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

        #region Shoot
        private void Shoot()
        {
            if (inputHandler.attackAction.triggered)
            {
                if(PlayerType == Data.PlayerType.Shooter)
                {
                    ShootBullet();
                }
                else if(PlayerType == Data.PlayerType.Supporter)
                {
                    ShootItem();
                }
            }
        }

        private void ShootBullet()
        {
            if(ammo > 0)
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

                ammo--;
            }
        }

        private void ShootItem()
        {
            if(itemHolder.childCount > 0)
            {
                GameObject itemBox = itemHolder.GetChild(0).gameObject;
                itemBox.transform.SetParent(null);

                Vector3 direction = (pointer.position - itemHolder.position).normalized;

                Rigidbody itemBoxRB = itemBox.GetComponent<Rigidbody>();
                if (itemBoxRB != null)
                {
                    itemBoxRB.isKinematic = false;
                    itemBoxRB.linearVelocity = direction * bulletSpeed;
                }

                itemBox.transform.forward = direction;
            }
        }
        #endregion

        private void MovePointer()
        {
            Vector3 screenPosition = new Vector3(inputHandler.mouseInput.x, inputHandler.mouseInput.y, distanceFromCamera);

            Vector3 targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            pointer.position = Vector3.Lerp(pointer.position, targetPosition, pointerSpeed * Time.deltaTime);
        }

        private void PickUp(GameObject item)
        {
            item.transform.SetParent(itemHolder);

            item.transform.localPosition = Vector3.zero;
            item.transform.localRotation = Quaternion.identity;

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            Collider col = item.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }

        public void AddAmmo(int ammo)
        {
            this.ammo += ammo;
        }
    }
}

