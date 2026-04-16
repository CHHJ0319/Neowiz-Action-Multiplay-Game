using Unity.Netcode;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

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
        [SerializeField] private UI.StageScene.Pointer pointer;
        //[SerializeField] private float distanceFromCamera = 10f;

        public float pickupRadius = 2.0f;
        [SerializeField] private Transform itemHolder;

        private Rigidbody rb;
        private PlayerInputHandler inputHandler;
        private PlayerAudioHandler audioHandler;
        private PlayerAnimationHandler animationHandler;
        private float lastAttackTime;

        public NetworkVariable<int> playerIndex = new NetworkVariable<int>();

        public NetworkVariable<int> Role = new NetworkVariable<int>();
        public NetworkVariable<int> Type = new NetworkVariable<int>();

        public NetworkVariable<bool> IsRoleChanged = new NetworkVariable<bool>();
        public NetworkVariable<int> Ammo = new NetworkVariable<int>();

        Vector3 velocity = new Vector3(0,0,0);
        private Vector3 targetPosition;
        public GameObject heldItem;

        private int defaultAmmo = 15;

        private NetworkVariable<Vector2> pointerPosition = new NetworkVariable<Vector2>(
            default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputHandler = GetComponent<PlayerInputHandler>();
            audioHandler = GetComponent<PlayerAudioHandler>();
            animationHandler = GetComponent<PlayerAnimationHandler>();
        }

        public override void OnNetworkSpawn()
        {
            Initialize(Utils.SceneNavigator.GetCurrentSceneName());

            IsRoleChanged.OnValueChanged += OnRoleAssigned;
            Ammo.OnValueChanged += OnAmmoChanged;

            if (IsOwner)
            {
                inputHandler.SetPlayerInputEnabled(true);
                SetPlayerIndexServerRPC(DataManager.Instance.SessionPlayerIndex);
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
                        ActorManager.Instance.SpawnItemServerRpc();
                    }
                }
            }
            else
            {
                if (pointer != null)
                {
                    pointer.transform.position = pointerPosition.Value;
                }
            }
        }

        private void FixedUpdate()
        {
            if (IsOwner)
            {
                Move();
                Rotate();
            }
        }

        #region Initailize
        public void Initialize(string sceneName)
        {       
            SetPointer((int)OwnerClientId);
            rb.useGravity = true;
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

            ShowPointer();
        }

        [Rpc(SendTo.Server)]
        private void SetPlayerIndexServerRPC(int index, RpcParams rpcParams = default)
        {
            playerIndex.Value = index;
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

        private void Move()
        {
            animationHandler.PlayMovement(inputHandler.horizontal, inputHandler.vertical);
            rb.linearVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

            transform.LookAt(targetPosition);
        }

        private void Rotate()
        {
            Vector3 lookAtPosition = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
            transform.LookAt(lookAtPosition);
        }

        #region Shoot
        private void Shoot()
        {
            if (inputHandler.attackAction.triggered)
            {
                audioHandler.PlayAttackSound();

                Vector3 direction = (targetPosition - itemHolder.position).normalized;

                if (Role.Value == (int)Data.PlayerRole.Shooter)
                {
                    direction.y = 0;
                    direction = direction.normalized;

                    ShootBulletServerRPC(
                        DataManager.Instance.SessionPlayerIndex,
                        DataManager.Instance.PlayerName,
                        direction, 
                        firePoint.position, 
                        firePoint.rotation);
                }
                else if (Role.Value == (int)Data.PlayerRole.Supporter)
                {
                    ShootItemServerRPC(direction);
                }
            }

        }

        [Rpc(SendTo.Server)]
        private void ShootBulletServerRPC(int playerIndex, string playerName, Vector3 direction, Vector3  spawnPosition, Quaternion spawnRotation, RpcParams rpcParams = default)
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

                bullet.GetComponent<Actor.Weapon.NetworkBullet>().Initialize(
                    playerName,
                    (Data.ElementType)Type.Value, 
                    direction * bulletSpeed);

                animationHandler.PlayAttack();
                Ammo.Value--;

                lastAttackTime = Time.time;
            }
            else
            {
                UIManager.Instance.UpdatePingMessageServerRpc(
                    playerIndex,
                    Data.RequestData.MessageList[0]);
            }
        }

        [Rpc(SendTo.Server)]
        private void ShootItemServerRPC(Vector3 direction, RpcParams rpcParams = default)
        {
            if (heldItem == null) return;

            heldItem.transform.SetParent(null);
            heldItem.transform.forward = direction;

            Rigidbody itemBoxRB = heldItem.GetComponent<Rigidbody>();
            if (itemBoxRB != null)
            {
                itemBoxRB.constraints = RigidbodyConstraints.None;


                itemBoxRB.AddForce((direction).normalized * throwForce, ForceMode.Impulse);
            }

            heldItem = null;
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
                    pointer.SetPosition(Camera.main.WorldToScreenPoint(targetPosition));
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

        public void ShowPointer()
        {
            pointer.gameObject.SetActive(true);
            if (Role.Value == (int)Data.PlayerRole.Shooter)
            {
                switch ((Data.ElementType)Type.Value)
                {
                    case Data.ElementType.Red:
                        pointer.SetColor(Color.red);
                        break;
                    case Data.ElementType.Green:
                        pointer.SetColor(Color.green);
                        break;
                    case Data.ElementType.Blue:
                        pointer.SetColor(Color.blue);
                        break;
                }
            }
            else if (Role.Value == (int)Data.PlayerRole.Supporter)
            {
                pointer.SetColor(Color.white);
            }
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
            if (heldItem != null) return;

            Collider[] colliders = Physics.OverlapSphere(transform.position, pickupRadius);

            Actor.Item.ItemBox closestItem = null;
            float minDistance = Mathf.Infinity;

            foreach (Collider col in colliders)
            {
                if (col.CompareTag("Item"))
                {
                    float distance = Vector3.Distance(transform.position, col.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        closestItem = col.GetComponent<Actor.Item.ItemBox>();
                    }
                }
            }

            if (closestItem != null)
            {
                NetworkObject netObj = closestItem.GetComponent<NetworkObject>();
                netObj.TrySetParent(transform, true);
                netObj.transform.localPosition = itemHolder.localPosition;
                netObj.transform.localRotation = Quaternion.identity;

                heldItem = closestItem.gameObject;
                
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, pickupRadius);
        }
        #endregion

        public void AddAmmo(int ammo)
        {
            Ammo.Value += ammo;
        }
    }
}

