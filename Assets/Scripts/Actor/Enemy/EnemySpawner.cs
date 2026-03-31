using UnityEngine;

namespace Actor.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Enemy Prepabs")]
        public GameObject normalEnemyPrefab;

        private float normalEnemySpawnRate = 70f;
        private float multiLivesEnemySpawnRate = 90f;
        private float multiTypeEnemySpawnRate = 100f;

        void Awake()
        {

        }

        private void OnEnable()
        {
            Events.ActorEvents.OnSpawnNormalRequested += SpawnNormalEnemy;
            Events.ActorEvents.OnSpawnMultiLivesRequested += SpawnMultiLivesEnemy;
            Events.ActorEvents.OnSpawnMultiTypeRequested += SpawnMultiTypeEnemy;
        }

        private void OnDisable()
        {
            Events.ActorEvents.OnSpawnNormalRequested -= SpawnNormalEnemy;
            Events.ActorEvents.OnSpawnMultiLivesRequested -= SpawnMultiLivesEnemy;
            Events.ActorEvents.OnSpawnMultiTypeRequested -= SpawnMultiTypeEnemy;
        }

        #region Spawn Enemy
        public void SpawnRandomEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            float randomRate = Random.Range(0f, 100f);

            if (randomRate < normalEnemySpawnRate)
            {
                SpawnNormalEnemy(spawnPosition, direction);
            }
            else if (randomRate < multiLivesEnemySpawnRate)
            {
                SpawnMultiLivesEnemy(spawnPosition, direction);
            }
            else if (randomRate < multiTypeEnemySpawnRate)
            {
                SpawnMultiTypeEnemy(spawnPosition, direction);
            }
        }

        public void SpawnNormalEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }

        public void SpawnMultiLivesEnemy(Vector3 spawnPosition, Vector3 direction)
        {

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultipleLives();
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }

        public void SpawnMultiTypeEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultiType();
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }
        #endregion

        
    }
}