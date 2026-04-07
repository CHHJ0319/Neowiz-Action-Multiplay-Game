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
                SpawnNormalEnemy(spawnPosition, direction, Data.ElementType.Random);
            }
            else if (randomRate < multiLivesEnemySpawnRate)
            {
                SpawnMultiLivesEnemy(spawnPosition, direction, Data.ElementType.Random);
            }
            else if (randomRate < multiTypeEnemySpawnRate)
            {
                SpawnMultiTypeEnemy(spawnPosition, direction, System.Array.Empty<Data.ElementType>());
            }
        }

        private void SpawnNormalEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetType(type);
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }

        private void SpawnMultiLivesEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type)
        {

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetType(type);
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultipleLives();
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }

        private void SpawnMultiTypeEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType[] types)
        {
            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultiType(types);
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(direction);
        }
        #endregion

        
    }
}