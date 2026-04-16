using Unity.Netcode;
using UnityEngine;
using UnityEngine.EventSystems;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

namespace Actor.Enemy
{
    public class EnemyController : NetworkBehaviour
    {
        public MeshRenderer elementIcon;

        public float damage = 10;
        public float speed = 5;

        public Material[] typeMaterials;

        private Rigidbody rb;

        private Actor.Enemy.EnemyAnimationHandler animationHandler;
        private Actor.Enemy.EnemyAudioHandler audioHandler;

        private int hp;

        public NetworkVariable<Data.NetworkElementType> Type = new NetworkVariable<Data.NetworkElementType>();
        public NetworkVariable<bool> IsMoving = new NetworkVariable<bool>();
        public NetworkVariable<Vector3> Direction = new NetworkVariable<Vector3>();


        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            animationHandler = GetComponent<Actor.Enemy.EnemyAnimationHandler>();
            audioHandler = GetComponent<Actor.Enemy.EnemyAudioHandler>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                ActorManager.Instance.AddEnemy(NetworkObjectId, this);
                animationHandler.OnDeathAnimationFinished += DespawnSelf;
            }

            Type.OnValueChanged += OnTypeChanged;
        }

        public override void OnNetworkDespawn()
        {
            if(IsServer)
            {
                ActorManager.Instance.RemoveEnemy(NetworkObjectId);
                animationHandler.OnDeathAnimationFinished -= DespawnSelf;
            }

            Type.OnValueChanged -= OnTypeChanged;
        }

        void FixedUpdate()
        {
            Move();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Barricade"))
            {
                if(IsServer)
                {
                    Events.ActorEvents.HandleEnemyPlayerFieldCollision(damage);
                    DespawnSelf();
                }
            }
            else if(other.CompareTag("DespawnWall"))
            {
                if (IsServer)
                {
                    DespawnSelf();
                }
            }
        }

        [Rpc(SendTo.Server)]
        public void SetTypeServerRPC(Data.ElementType type, RpcParams rpcParams = default)
        {
            SetType(type);
        }

        public void SetType(Data.ElementType type)
        {
            if(type == Data.ElementType.Random)
            {
                SetRandomElementType();
            }
            else
            {
                Type.Value = type;
            }
        }

        private void OnTypeChanged(Data.NetworkElementType previousValue, Data.NetworkElementType newValue)
        {
            SetElemetIcon();
        }

        private void SetElemetIcon()
        {
            if (elementIcon != null)
            {
                switch ((Data.ElementType)Type.Value)
                {
                    case Data.ElementType.Red:
                        elementIcon.material = typeMaterials[0];
                        break;
                    case Data.ElementType.Green:
                        elementIcon.material = typeMaterials[1];
                        break;
                    case Data.ElementType.Blue:
                        elementIcon.material = typeMaterials[2];
                        break;
                    case Data.ElementType.Yellow:
                        elementIcon.material = typeMaterials[0];
                        break;
                    case Data.ElementType.Magenta:
                        elementIcon.material = typeMaterials[1];
                        break;
                    case Data.ElementType.Cyan:
                        elementIcon.material = typeMaterials[2];
                        break;
                }
            }
        }

        public void SetHP(int lives)
        {
            this.hp = lives;
        }

        public void SetMultiType(Data.ElementType type)
        {
            if (type == Data.ElementType.Random)
            {
                SetRandomMultiType();
            }
            else
            {
                Type.Value = type;
            }
        }

        public void TakeDamage(Data.ElementType bulletType, string playerName)
        {
            //if(Type.Value == bulletType)
            //{
                hp--;
            //}

            if (hp <= 0)
            {
                DieServerRPC(playerName);
            }
        }

        [Rpc(SendTo.Server)]
        public void StartMovingServerRpc(Vector3 direction, RpcParams rpcParams = default)
        {
            IsMoving.Value = true;
            Direction.Value = direction;
        }

        public void Move()
        {
            if(IsMoving.Value)
            {
                if (rb != null)
                {
                    rb.linearVelocity = Direction.Value.normalized * speed;
                }
            }
            else
            {
                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                }
            }
        }

        [Rpc(SendTo.Server)]
        public void DieServerRPC(string playerName, RpcParams rpcParams = default)
        {
            IsMoving.Value = false;
            audioHandler.PlayDeathSound();
            animationHandler.PlayDead();

            StageManager.Instance.UpdateScoreServerRpc(playerName);
        }

        public void DespawnSelf()
        {
            if (!IsSpawned)
            {
                return;
            }

            GetComponent<NetworkObject>().Despawn();
        }

        private void SetRandomElementType()
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);

            Type.Value = (Data.ElementType) randomIndex;
        }

        private void SetRandomMultiType()
        {
            int randomIndex = UnityEngine.Random.Range(3, 6);

            Type.Value = (Data.ElementType) randomIndex;
        }
    }
}