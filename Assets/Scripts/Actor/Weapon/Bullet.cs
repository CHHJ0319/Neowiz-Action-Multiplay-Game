using UnityEngine;

namespace Actor.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float lifeTime = 3f;

        [Header("Materials")]
        public Material typeRed;
        public Material typeGreen;
        public Material typeBlue;

        public Data.ElementType Type { get; private set; }

        private MeshRenderer meshRenderer;

        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy.EnemyController enemy = other.GetComponent<Enemy.EnemyController>();
                enemy.TakeDamage(Type);
                Destroy(gameObject);
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
    }
}

