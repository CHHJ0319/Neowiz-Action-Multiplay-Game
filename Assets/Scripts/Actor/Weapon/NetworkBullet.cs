using Unity.Netcode;
using UnityEngine;
using static UnityEngine.LowLevelPhysics2D.PhysicsLayers;

namespace Actor.Weapon
{
    public class NetworkBullet : NetworkBehaviour
    {
        public float lifeTime = 3f;

        [SerializeField] private GameObject fakeBulletPrefab;

        public Data.ElementType type;

        private string playerName;

        private Bullet fakeBullet;

        public NetworkVariable<Vector3> velocity = new NetworkVariable<Vector3>();


        private void Awake()
        {
        }

        public override void OnNetworkSpawn()
        {
            velocity.OnValueChanged += Launch;
        }

        public override void OnNetworkDespawn()
        {
            if (fakeBullet != null)
            {
                Destroy(fakeBullet.gameObject);
            }

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
                enemy.TakeDamage(type, playerName);
                if (IsServer)
                {
                    DespawnBullet();
                }
            }
            else if (other.CompareTag("Item"))
            {
                if (IsServer)
                {
                    DespawnBullet();
                }
            }
        }

        public void Initialize(string playerName, Data.ElementType type, Vector3 velocity)
        {
            this.playerName = playerName;
            this.type = type;
            this.velocity.Value = velocity;
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

