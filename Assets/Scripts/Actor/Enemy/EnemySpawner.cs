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
        public Camera mainCam;
        public float fixedSpawnY = 0.5f;
        public float offScreenOffset = 1.1f;
        public float negativeOffset = -0.1f;

        private float normalEnemySpawnRate = 40f;
        private float multiLivesEnemySpawnRate = 90f;
        private float multiTypeEnemySpawnRate = 100f;

        void Awake()
        {
            if (mainCam == null) mainCam = Camera.main;
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
                else if (randomRate < multiLivesEnemySpawnRate)
                {
                    SpawnerMultiLivesEnemy();
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

        public void SpawnerMultiLivesEnemy()
        {
            Vector3 spawnPosition = GetRandomSpawnPosition();
            Vector3 direction = (target.localPosition - spawnPosition).normalized;

            GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
            enemy.GetComponent<Actor.Enemy.EnemyController>().SetMultipleLives();
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
            float viewportX = 0f;
            float viewportY = 0f;

            int side = Random.Range(0, 3);

            switch (side)
            {
                case 0:
                    viewportX = Random.Range(negativeOffset, offScreenOffset);
                    viewportY = offScreenOffset;
                    break;

                case 1:
                    viewportX = negativeOffset;
                    viewportY = Random.Range(0f, offScreenOffset);
                    break;

                case 2:
                    viewportX = offScreenOffset;
                    viewportY = Random.Range(0f, offScreenOffset);
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
