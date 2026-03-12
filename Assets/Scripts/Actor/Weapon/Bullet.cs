using UnityEngine;

namespace Actor.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public float lifeTime = 3f;

        void Start()
        {
            Destroy(gameObject, lifeTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                EnemyController enemy = other.GetComponent<EnemyController>();
                enemy.TakeDamage();
            }

            Destroy(gameObject);
        }
    }
}

