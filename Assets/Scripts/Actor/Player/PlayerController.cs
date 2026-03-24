using Unity.Netcode;
using UnityEditor.Timeline.Actions;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Player 
{
    public class PlayerController : NetworkBehaviour
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
        [SerializeField] private RectTransform pointer;
        [SerializeField] private float pointerSpeed = 10f;
        [SerializeField] private float distanceFromCamera = 10f;

        [SerializeField] private Transform itemHolder;

        private Rigidbody rb;
        private PlayerInputHandler inputHandler;

        public Data.PlayerType PlayerType { get; private set; } = Data.PlayerType.Shooter;
        public int ammo;

        Vector3 velocity = new Vector3(0,0,0);

        private Vector3 targetPosition;
        private GameObject targetItem;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputHandler = GetComponent<PlayerInputHandler>();

            ammo = 100;
        }

        private void OnEnable()
        {
            inputHandler.interactAction.performed += Interact;

            Events.RoundEvents.OnRoundStarted += ShowPointer;
            Events.RoundEvents.OnRoundEnded += HidePointer;
        }

        private void OnDisable()
        {
            inputHandler.interactAction.performed -= Interact;

            Events.RoundEvents.OnRoundStarted -= ShowPointer;
            Events.RoundEvents.OnRoundEnded -= HidePointer;
        }

        public override void OnNetworkSpawn()
        {
            if (!IsOwner)
            {
                GetComponent<PlayerInput>().enabled = false;
                return;
            }
        }

        public override void OnNetworkDespawn()
        {
        }

        private void Update()
        {
            CalculateVeocity();
            MovePointer();
            Shoot();
        }

        private void FixedUpdate()
        {
            ApplyMovement();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Item"))
            {
                GameObject item = other.gameObject;
                targetItem = item;
            }
        }

        private void CalculateVeocity()
        {
            Vector3 moveDirection = new Vector3(inputHandler.horizontal, 0, inputHandler.vertical);
            float currentSpeed = inputHandler.isDashPressed ? dashSpeed : walkSpeed;
            velocity = moveDirection * currentSpeed;
        }

        private void ApplyMovement()
        {
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

            //if (moveDirection.magnitude > 0.1f)
            //{
            //    Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            //    rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime));
            //}
        }

        #region Shoot
        private void Shoot()
        {
            if(inputHandler.attackAction.triggered)
            {
                if (PlayerType == Data.PlayerType.Shooter)
                {
                    ShootBullet();
                }
                else if (PlayerType == Data.PlayerType.Supporter)
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

                Vector3 direction = (targetPosition - firePoint.position).normalized;
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

                Vector3 direction = (targetPosition - itemHolder.position).normalized;

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
            targetPosition = Camera.main.ScreenToWorldPoint(screenPosition);

            Vector2 mousePosition = Mouse.current.position.ReadValue();
            pointer.position = mousePosition;
        }

        private void Interact(InputAction.CallbackContext context)
        {
            if(PlayerType == Data.PlayerType.Supporter)
            {
                PickUp();
            }
        }

        private void PickUp()
        {
            if (targetItem == null) return;

            targetItem.transform.SetParent(itemHolder);

            targetItem.transform.localPosition = Vector3.zero;
            targetItem.transform.localRotation = Quaternion.identity;

            Rigidbody rb = targetItem.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }

            Collider col = targetItem.GetComponent<Collider>();
            if (col != null)
            {
                col.enabled = false;
            }
        }

        public void AddAmmo(int ammo)
        {
            this.ammo += ammo;
        }

        public void SetPointer(int playerIndex)
        {
            pointer = UIManager.Instance.GetPointer(playerIndex);
        }

        public void SetPosition(Vector3 pos)
        {
            transform.localPosition = pos;
        }

        private void ShowPointer()
        {
            pointer.gameObject.SetActive(true);
        }

        private void HidePointer()
        {
            pointer.gameObject.SetActive(false);
        }
    }
}

