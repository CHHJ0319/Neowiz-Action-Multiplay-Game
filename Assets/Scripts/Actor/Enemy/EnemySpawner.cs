using Unity.Netcode;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        public static EnemySpawner Instance;

        [Header("Enemy Prepabs")]
        public GameObject normalEnemyPrefab;

        private float normalEnemySpawnRate = 70f;
        private float multiLivesEnemySpawnRate = 90f;
        private float multiTypeEnemySpawnRate = 100f;

        public float spawnWidth = 18f;
        private static float fixedSpawnY = 0.5f;

        void Awake()
        {
            Instance = this;
        }

        public void SpawnEnemyRow(int enemiesPerWave, bool isTargeting)
        {
            Transform target = PlayerField.Instance.core;

            float halfWidth = spawnWidth / 2f;
            Vector3 leftPos = transform.position + Vector3.left * halfWidth;
            float step = (enemiesPerWave > 1) ? spawnWidth / (enemiesPerWave - 1) : 0;

            for (int i = 0; i < enemiesPerWave; i++)
            {
                Vector3 spawnPosition = leftPos + (Vector3.right * step * i);
                spawnPosition.y = fixedSpawnY;

                Vector3 direction = Vector3.back;
                if (isTargeting)
                {
                    direction = (target.position - spawnPosition).normalized;

                }

                SpawnNormalEnemy(spawnPosition, direction, Data.ElementType.Random);
            }
        }

        #region Spawn Enemy
        public void SpawnRandomEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            float randomRate = Random.Range(0f, 100f);

            if (randomRate < normalEnemySpawnRate)
            {
                SpawnNormalEnemy(spawnPosition, direction, Data.ElementType.Random);
            }
            else if (randomRate < multiLivesEnemySpawnRate)
            {
                SpawnMultiLivesEnemy(spawnPosition, direction, Data.ElementType.Random, 3);
            }
            else if (randomRate < multiTypeEnemySpawnRate)
            {
                SpawnMultiTypeEnemy(spawnPosition, direction, Data.ElementType.Random);
            }
        }

        private void SpawnNormalEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));

            NetworkObject netObj = enemy.GetComponent<NetworkObject>();
            netObj.Spawn();

            enemy.GetComponent<Actor.Enemy.EnemyController>().SetTypeServerRPC(type);
            enemy.GetComponent<Actor.Enemy.EnemyController>().LaunchClientRpc(direction);
        }

        private void SpawnMultiLivesEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type, int lives)
        {

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));

            NetworkObject netObj = enemy.GetComponent<NetworkObject>();
            netObj.Spawn();

            enemy.GetComponent<Actor.Enemy.EnemyController>().SetType(type);
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetHP(lives);
            enemy.GetComponent<Actor.Enemy.EnemyController>().LaunchClientRpc(direction);
        }

        private void SpawnMultiTypeEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));

            NetworkObject netObj = enemy.GetComponent<NetworkObject>();
            netObj.Spawn();

            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultiType(type);
            enemy.GetComponent<Actor.Enemy.EnemyController>().LaunchClientRpc(direction);
        }
        #endregion

        
    }
}