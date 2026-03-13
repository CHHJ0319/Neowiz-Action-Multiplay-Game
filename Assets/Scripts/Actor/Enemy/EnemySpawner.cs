using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header ("Enemy Prepabs")]
        public GameObject normalEnemyPrefab;

        [Header("Spawn Range")]
        public float minX = -10f;
        public float maxX = 10f;
        public float minZ = -10f;
        public float maxZ = 10f;

        private float normalEnemySpawnRate = 70f;
        private float bigEnemySpawnRate = 20f;
        private float multiTypeEnemySpawnRate = 100f;

        void Start()
        {
        }

        void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                float randomRate = Random.Range(0f, 100f);

                if (randomRate < normalEnemySpawnRate)
                {
                    SpawnerNormalEnemy();
                }
                else if (randomRate < multiTypeEnemySpawnRate)
                {
                    SpawnerMultiTypeEnemy();
                }
            }
        }

        public void SpawnerNormalEnemy()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Vector3 direction = (target.localPosition - spawnPosition).normalized;

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }

        public void SpawnerMultiTypeEnemy()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Vector3 direction = (target.localPosition - spawnPosition).normalized;

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultiType();
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }

        private Vector3 GetRandomSpawnPosition()
        {
            float randomX = Random.Range(minX, maxX);
            float randomZ = Random.Range(minZ, maxZ);

            return new Vector3(randomX, 0.5f, randomZ);
        }
    }
}
