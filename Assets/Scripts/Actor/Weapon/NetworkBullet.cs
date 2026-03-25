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

        public bool isFake = false;

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public override void OnNetworkSpawn()
        {
            if (!IsServer)
            {
                GameObject fake = Instantiate(fakeBulletPrefab, transform.position, transform.rotation);
            }
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

        private void DespawnBullet()
        {
            if (IsSpawned)
            {
                GetComponent<NetworkObject>().Despawn();
            }
        }
    }
}

