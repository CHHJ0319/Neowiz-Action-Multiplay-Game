using UnityEngine;

namespace Actor.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header ("Enemy Prepabs")]
        public GameObject normalEnemyPrefab;

        [Header("Target")]
        public Transform target;

        [Header("Spawn Range")]
        public float minX = -10f;
        public float maxX = 10f;
        public float minZ = -10f;
        public float maxZ = 10f;

        void Start()
        {
            SpawnerNormalEnemy();
        }

        public void SpawnerNormalEnemy()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Vector3 direction = (target.localPosition - spawnPosition).normalized;

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            float speed = enemy.GetComponent<Actor.Enemy.EnemyController>().Speed;
            
            Rigidbody rb = enemy.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }

        public Vector3 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            return new Vector3(randomX, 0.5f, randomZ);
        }
    }
}
