using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [Header("Target")]
        public Transform target;

        [Header("Enemy Prepabs")]
        public GameObject normalEnemyPrefab;

        [Header("Spawn Range")]
        public Camera mainCam;
        public float fixedSpawnY = 0.5f;
        public float offScreenOffset = 1.1f;
        public float negativeOffset = -0.1f;

        private float normalEnemySpawnRate = 70f;
        private float multiLivesEnemySpawnRate = 90f;
        private float multiTypeEnemySpawnRate = 100f;

        void Awake()
        {
            if (mainCam == null) mainCam = Camera.main;
        }

        //void OnInteract(InputValue value)
        //{
        //    if (value.isPressed)
        //    {
        //        SpawnRandomPositionEnemy();
        //    }
        //}

        #region Spawn Enemy
        public void SpawnRandomPositionEnemy()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            SpawnRandomEnemy(spawnPosition);
        }

        public void SpawnRandomEnemy(Vector3 spawnPosition, Vector3? direction = null)
        {
            float randomRate = Random.Range(0f, 100f);

            if (randomRate < normalEnemySpawnRate)
            {
                SpawnerNormalEnemy(spawnPosition, direction);
            }
            else if (randomRate < multiLivesEnemySpawnRate)
            {
                SpawnerMultiLivesEnemy(spawnPosition, direction);
            }
            else if (randomRate < multiTypeEnemySpawnRate)
            {
                SpawnerMultiTypeEnemy(spawnPosition, direction);
            }
        }

        public void SpawnerNormalEnemy(Vector3 spawnPosition, Vector3? direction = null)
        {
            Vector3 finalDirection = (target.localPosition - spawnPosition).normalized;
            if (direction.HasValue)
            {
                finalDirection = direction.Value;
            }

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(finalDirection));
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(finalDirection);
        }

        public void SpawnerMultiLivesEnemy(Vector3 spawnPosition, Vector3? direction = null)
        {
            Vector3 finalDirection = (target.localPosition - spawnPosition).normalized;
            if (direction.HasValue)
            {
                finalDirection = direction.Value;
            }

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(finalDirection));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultipleLives();
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(finalDirection);
        }

        public void SpawnerMultiTypeEnemy(Vector3 spawnPosition, Vector3? direction = null)
        {
            Vector3 finalDirection = (target.localPosition - spawnPosition).normalized;
            if (direction.HasValue)
            {
                finalDirection = direction.Value;
            }

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(finalDirection));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultiType();
            enemy.GetComponent<Actor.Enemy.EnemyController>().Launch(finalDirection);
        }

        #endregion

        private Vector3 GetRandomSpawnPosition()
        {
            float viewportX = 0f;
            float viewportY = 0f;

            int side = UnityEngine.Random.Range(0, 3);

            switch (side)
            {
                case 0:
                    viewportX = UnityEngine.Random.Range(negativeOffset, offScreenOffset);
                    viewportY = offScreenOffset;
                    break;

                case 1:
                    viewportX = negativeOffset;
                    viewportY = UnityEngine.Random.Range(0f, offScreenOffset);
                    break;

                case 2:
                    viewportX = offScreenOffset;
                    viewportY = UnityEngine.Random.Range(0f, offScreenOffset);
                    break;
            }

            Ray ray = mainCam.ViewportPointToRay(new Vector3(viewportX, viewportY, 0));
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedSpawnY, 0));
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return new Vector3(0, fixedSpawnY, 0);
        }
    }
}