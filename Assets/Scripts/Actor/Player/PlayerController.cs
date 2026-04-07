using Actor.Item;
using Actor.Spawner;
using Actor.Weapon;
using System;
using Unity.Netcode;
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
        [SerializeField] private GameObject networkBulletPrefab;
        [SerializeField] private Transform firePoint;
        [SerializeField] private float bulletSpeed = 20f;
        [SerializeField] private float throwForce = 10f;

        [Header("Pointer Settings")]
        [SerializeField] private RectTransform pointer;
        //[SerializeField] private float distanceFromCamera = 10f;

        [SerializeField] private Transform itemHolder;

        private Rigidbody rb;
        private PlayerInputHandler inputHandler;
        private PlayerAudioHandler audioHandler;
        private PlayerAnimationHandler animationHandler;

       public NetworkVariable<Data.PlayerInfo> PlayerInfo { get; private set; }
            = new NetworkVariable<Data.PlayerInfo>( new Data.PlayerInfo { 
                playerName = "Player",
                character = Data.CharacterType.One, 
                role = Data.PlayerRole.Shooter, 
                color = Data.ElementType.Red
            });  

        public int ammo;
        Vector3 velocity = new Vector3(0,0,0);
        private Vector3 targetPosition;
        public GameObject targetItem;

        private NetworkVariable<Vector2> pointerPosition = new NetworkVariable<Vector2>(
            default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            inputHandler = GetComponent<PlayerInputHandler>();
            audioHandler = GetComponent<PlayerAudioHandler>();
            animationHandler = GetComponent<PlayerAnimationHandler>();

            ammo = 100;
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
            SetPlayerName();
            Initialize(Utils.SceneNavigator.GetCurrentSceneName());

            NetworkManager.SceneManager.OnLoadComplete += OnSceneLoaded;

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
            NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoaded;
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
                        ItemSpawner.Instance.SpawnItemServerRpc();
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
            if (sceneName == Utils.SceneList.LobbyScene.ToString())
            {
                Events.PlayerEvents.InitializeInLobbyScene(PlayerInfo.Value.playerName, (int)OwnerClientId, IsOwner);
            }
            else if (sceneName == Utils.SceneList.TutorialScene.ToString())
            {
                if(IsOwner)
                {
                    Events.PlayerEvents.InitializeInStageScene(PlayerInfo.Value);
                }
                SetPointer((int)OwnerClientId);
                rb.useGravity = true;
            }
        }

        private void SetPlayerName()
        {
            var currentInfo = PlayerInfo.Value;
            currentInfo.playerName = "Player" + (OwnerClientId + 1);

            PlayerInfo.Value = currentInfo;
        }

        public void SetChareacter(int index)
        {
            var currentInfo = PlayerInfo.Value;
            currentInfo.character = (Data.CharacterType)index;

            PlayerInfo.Value = currentInfo;
        }

        public void SetRole(Data.PlayerRole role, Data.ElementType color)
        {
            var currentInfo = PlayerInfo.Value;
            currentInfo.role = role;
            currentInfo.color = color;

            PlayerInfo.Value = currentInfo;

            Events.PlayerEvents.AssignPlayerRole(PlayerInfo.Value);
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
                if (PlayerInfo.Value.role == Data.PlayerRole.Shooter)
                {
                    direction.y = 0;
                    direction = direction.normalized;

                    ShootBulletServerRPC(direction, firePoint.position, firePoint.rotation);
                }
                else if (PlayerInfo.Value.role == Data.PlayerRole.Supporter)
                {
                    ShootItemServerRPC(direction);
                }
            }
        }

        [Rpc(SendTo.Server)]
        private void ShootBulletServerRPC(Vector3 direction, Vector3  spawnPosition, Quaternion spawnRotation, RpcParams rpcParams = default)
        {
            if(ammo > 0)
            {
                GameObject bullet = Instantiate(networkBulletPrefab, spawnPosition, spawnRotation);

                NetworkObject netObj = bullet.GetComponent<NetworkObject>();
                netObj.Spawn();

                bullet.GetComponent<NetworkBullet>().Intialize(PlayerInfo.Value.color, direction * bulletSpeed);

                audioHandler.PlayAttackSound();
                animationHandler.PlayAttack();
                ammo--;
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
                if (PlayerInfo.Value.role == Data.PlayerRole.Shooter)
                {
                    
                }
                else if (PlayerInfo.Value.role == Data.PlayerRole.Supporter)
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
            this.ammo += ammo;
        }

        private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadMode)
        {
            Initialize(sceneName);
        }
    }
}

