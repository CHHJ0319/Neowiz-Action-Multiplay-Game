using System.Data;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Actor.Player 
{
    public class PlayerController : NetworkBehaviour
    {
        [Header("Movement Settings")]
        [SerializeField] private float walkSpeed = 5f;
        [SerializeField] private float dashSpeed = 10f;

        [Header("Attack Settings")]
        public GameObject greenBulletPrefab;
        public GameObject redBulletPrefab;
        public GameObject blueBulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float throwForce = 10f;
        [SerializeField] private float attackCooldown = 0.5f;

        [Header("Pointer Settings")]
        [SerializeField] private RectTransform pointer;
        //[SerializeField] private float distanceFromCamera = 10f;

        [SerializeField] private Transform itemHolder;

        private Rigidbody rb;
        private PlayerInputHandler inputHandler;
        private PlayerAudioHandler audioHandler;
        private PlayerAnimationHandler animationHandler;
        private float lastAttackTime;

        public NetworkVariable<int> Role = new NetworkVariable<int>();
        public NetworkVariable<int> Type = new NetworkVariable<int>();

        public NetworkVariable<bool> IsRoleChanged = new NetworkVariable<bool>();
        public NetworkVariable<int> Ammo = new NetworkVariable<int>();

        Vector3 velocity = new Vector3(0,0,0);
        private Vector3 targetPosition;
        public GameObject targetItem;

        private int defaultAmmo = 30;

        private NetworkVariable<Vector2> pointerPosition = new NetworkVariable<Vector2>(
            default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputHandler = GetComponent<PlayerInputHandler>();
            audioHandler = GetComponent<PlayerAudioHandler>();
            animationHandler = GetComponent<PlayerAnimationHandler>();
        }

        private void OnEnable()
        {
            Events.RoundEvents.OnRoundStarted += ShowPointer;
        }

        private void OnDisable()
        {
            Events.RoundEvents.OnRoundStarted -= ShowPointer;
        }

        public override void OnNetworkSpawn()
        {
            Initialize(Utils.SceneNavigator.GetCurrentSceneName());

            IsRoleChanged.OnValueChanged += OnRoleAssigned;
            Ammo.OnValueChanged += OnAmmoChanged;

            if (IsOwner)
            {
                inputHandler.SetPlayerInputEnabled(true);
            }
            else
            {
                inputHandler.SetPlayerInputEnabled(false);
            }
        }

        public override void OnNetworkDespawn()
        {
            IsRoleChanged.OnValueChanged -= OnRoleAssigned;
            Ammo.OnValueChanged -= OnAmmoChanged;
        }

        private void Update()
        {
            if (IsOwner)
            {
                if(Utils.SceneNavigator.GetCurrentSceneName() != Utils.SceneList.LobbyScene.ToString())
                {
                    CalculateVeocity();
                    Shoot();
                    Interact();
                    MovePointerLocal();
                    UpdatePointerServerRpc(inputHandler.mouseInput);

                    if (inputHandler.quickSlot1Action.triggered)
                    {
                        Actor.Spawner.ItemSpawner.Instance.SpawnItemServerRpc();
                    }
                }
            }
            else
            {
                if (pointer != null)
                {
                    pointer.position = pointerPosition.Value;
                }
            }
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

        private void OnTriggerExit(Collider other)
        {
            targetItem = null;
        }

        #region Initailize
        public void Initialize(string sceneName)
        {       
            if (sceneName == Utils.SceneList.TutorialScene.ToString())
            {
                SetPointer((int)OwnerClientId);
                rb.useGravity = true;
            }
        }

        public void SetRole(Data.PlayerRole role, Data.ElementType type)
        {
            if(role == Data.PlayerRole.Shooter)
            {
                Ammo.Value = defaultAmmo;
            }
            else if (role == Data.PlayerRole.Supporter)
            {
                Ammo.Value = 0;
            }

            Role.Value = (int)role;
            Type.Value = (int)type;

            IsRoleChanged.Value = !IsRoleChanged.Value;
        }

        public void OnRoleAssigned(bool previousValue, bool newValue)
        {
            if (IsOwner)
            {
                Data.PlayerRole role = (Data.PlayerRole)Role.Value;
                Data.ElementType type = (Data.ElementType)Type.Value;
                Events.PlayerEvents.UpdateRoleUI(role, type);
            }
        }
        #endregion

        private void CalculateVeocity()
        {
            if(inputHandler.dashAction.triggered)
            {
                audioHandler.PlayDashSound();
            }
                Vector3 moveDirection = new Vector3(inputHandler.horizontal, 0, inputHandler.vertical);
            float currentSpeed = inputHandler.isDashPressed ? dashSpeed : walkSpeed;
            velocity = moveDirection * currentSpeed;
        }

        private void ApplyMovement()
        {
            animationHandler.PlayMovement(inputHandler.horizontal, inputHandler.vertical);
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
            Vector3 direction = (targetPosition - itemHolder.position).normalized;

            if (inputHandler.attackAction.triggered)
            {
                if (Role.Value == (int)Data.PlayerRole.Shooter)
                {
                    direction.y = 0;
                    direction = direction.normalized;

                    ShootBulletServerRPC(direction, firePoint.position, firePoint.rotation);
                }
                else if (Role.Value == (int)Data.PlayerRole.Supporter)
                {
                    ShootItemServerRPC(direction);
                }
            }

        }

        [Rpc(SendTo.Server)]
        private void ShootBulletServerRPC(Vector3 direction, Vector3  spawnPosition, Quaternion spawnRotation, RpcParams rpcParams = default)
        {
            if (Time.time - lastAttackTime < attackCooldown) return;

            if (Ammo.Value > 0)
            {
                GameObject bullet = null;
                switch (Type.Value)
                {
                    case (int)Data.ElementType.Red: 
                        bullet = Instantiate(redBulletPrefab, spawnPosition, spawnRotation); 
                        break;
                    case (int)Data.ElementType.Green:
                        bullet = Instantiate(greenBulletPrefab, spawnPosition, spawnRotation);
                        break;
                    case (int)Data.ElementType.Blue:
                        bullet = Instantiate(blueBulletPrefab, spawnPosition, spawnRotation);
                        break;
                }

                NetworkObject netObj = bullet.GetComponent<NetworkObject>();
                netObj.Spawn();

                bullet.GetComponent<Actor.Weapon.NetworkBullet>().Initialize((Data.ElementType)Type.Value, direction * bulletSpeed);

                audioHandler.PlayAttackSound();
                animationHandler.PlayAttack();
                Ammo.Value--;

                lastAttackTime = Time.time;
            }
        }

        [Rpc(SendTo.Server)]
        private void ShootItemServerRPC(Vector3 direction, RpcParams rpcParams = default)
        {
            if(targetItem != null)
            {
                audioHandler.PlayAttackSound();

                targetItem.transform.SetParent(null);
                targetItem.transform.forward = direction;

                Rigidbody itemBoxRB = targetItem.GetComponent<Rigidbody>();
                if (itemBoxRB != null)
                {
                    itemBoxRB.isKinematic = false;

                    itemBoxRB.constraints = RigidbodyConstraints.None;
                    itemBoxRB.constraints = RigidbodyConstraints.FreezeRotation;

                    itemBoxRB.linearVelocity = direction * throwForce;
                }
            }
        }

        public void OnAmmoChanged(int previousValue, int newValue)
        {
            if (IsOwner)
            {
                Events.PlayerEvents.UpdateAmmoUI(Ammo.Value); 
            }
        }
        #endregion

        #region Pointer
        private void MovePointerLocal()
        {
            Ray ray = Camera.main.ScreenPointToRay(inputHandler.mouseInput);
            Plane groundPlane = new Plane(Vector3.up, Vector3.zero);

            if (groundPlane.Raycast(ray, out float enter))
            {
                targetPosition = ray.GetPoint(enter);

                if (pointer != null)
                {
                    pointer.position = Camera.main.WorldToScreenPoint(targetPosition);
                }
            }
        }

        [Rpc(SendTo.Server)]
        private void UpdatePointerServerRpc(Vector2 pos, RpcParams rpcParams = default)
        {
            pointerPosition.Value = pos;
        }

        private void SetPointer(int id)
        {
            pointer = UIManager.Instance.GetPointer(id);
        }

        private void ShowPointer()
        {
            pointer.gameObject.SetActive(true);
        }
        #endregion

        #region Interact
        private void Interact()
        {
            if(inputHandler.interactAction.triggered)
            {
                if (Role.Value == (int)Data.PlayerRole.Shooter)
                {
                    
                }
                else if (Role.Value == (int)Data.PlayerRole.Supporter)
                {
                    PickUpServerRpc();
                }
            }
        }

        [Rpc(SendTo.Server)]
        private void PickUpServerRpc(RpcParams rpcParams = default)
        {
            if (targetItem == null) return;
            Rigidbody itemBoxRB = targetItem.GetComponent<Rigidbody>();
            if (itemBoxRB != null)
            {
                itemBoxRB.isKinematic = true;
            }

            NetworkObject netObj = targetItem.GetComponent<NetworkObject>();
            netObj.TrySetParent(transform, true);
            netObj.transform.localPosition = itemHolder.localPosition;
            netObj.transform.localRotation = Quaternion.identity;

            //Collider col = targetItem.GetComponent<Collider>();
            //if (col != null)
            //{
            //    col.enabled = false;
            //}
        }
        #endregion

        public void AddAmmo(int ammo)
        {
            Ammo.Value += ammo;
        }
    }
}

