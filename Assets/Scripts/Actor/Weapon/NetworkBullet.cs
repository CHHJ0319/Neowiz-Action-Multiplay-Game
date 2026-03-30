using Unity.Netcode;
using UnityEngine;

namespace Actor.Weapon
{
    public class NetworkBullet : NetworkBehaviour
    {
        public float lifeTime = 3f;

        [SerializeField] private GameObject fakeBulletPrefab;

        [Header("Materials")]
        public Material typeRed;
        public Material typeGreen;
        public Material typeBlue;

        public Data.ElementType Type { get; private set; }

        private MeshRenderer meshRenderer;

        private Bullet fakeBullet;

        public NetworkVariable<Vector3> velocity = new NetworkVariable<Vector3>(
            default, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Server);

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override void OnNetworkSpawn()
        {
            velocity.OnValueChanged += Launch;
        }

        public override void OnNetworkDespawn()
        {
            velocity.OnValueChanged -= Launch;
        }

        void Start()
        {
            if (IsServer)
            {
                Invoke(nameof(DespawnBullet), lifeTime);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy.EnemyController enemy = other.GetComponent<Enemy.EnemyController>();
                enemy.TakeDamage(Type);
                if (IsServer)
                {
                    DespawnBullet();
                }
            }
        }

        public void Intialize(Data.ElementType type)
        {
            Type = type;

            switch (Type)
            {
                case Data.ElementType.Red:
                    meshRenderer.material = typeRed;
                    break;
                case Data.ElementType.Green:
                    meshRenderer.material = typeGreen;
                    break;
                case Data.ElementType.Blue:
                    meshRenderer.material = typeBlue;
                    break;
            }
        }

        private void Launch(Vector3 previousValue, Vector3 newValue)
        {
            if (IsServer)
            {
                GetComponent<Rigidbody>().linearVelocity = velocity.Value;
                transform.forward = velocity.Value;
            }
            else
            {
                gameObject.SetActive(false);

                GameObject fake = Instantiate(fakeBulletPrefab, transform.position, transform.rotation);
                fakeBullet = fake.GetComponent<Bullet>();
                fakeBullet.Launch(velocity.Value);
            }

            
        }

        private void DespawnBullet()
        {
            if (IsSpawned)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}

