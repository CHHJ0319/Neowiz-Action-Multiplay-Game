using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

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
                float randomRate = UnityEngine.Random.Range(0f, 100f);

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

        void Update()
        {
            if (Keyboard.current.digit6Key.wasPressedThisFrame)
            {
                StartCoroutine(SpawnAllPatternsRoutine());
            }
        }

        private IEnumerator SpawnAllPatternsRoutine()
        {
            StartCoroutine(SpawnGapWall(8, 1));

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(SpawnArrowheadAssault(1));

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(SpawnSweepingWave(8));

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(SpawnPincerAttack(3));

            yield return new WaitForSeconds(1.5f);

            StartCoroutine(SpawnMeteorRain(10));
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

        private void ApplyRandomAttribute(Actor.Enemy.EnemyController controller)
        {
            float randomRate = UnityEngine.Random.Range(0f, 100f);

            if (randomRate >= normalEnemySpawnRate && randomRate < multiLivesEnemySpawnRate)
            {
                controller.SetMultipleLives();
            }
            else if (randomRate >= multiLivesEnemySpawnRate)
            {
                controller.SetMultiType();
            }
        }

        private IEnumerator SpawnGapWall(int enemiesPerWave, int waves)
        {
            float startX = 0f;
            float endX = 1f;
            float step = (endX - startX) / (enemiesPerWave - 1);
            int lastGap = -1;

            for (int w = 0; w < waves; w++)
            {
                int gapIndex;
                do
                {
                    gapIndex = UnityEngine.Random.Range(0, enemiesPerWave - 1);
                } while (gapIndex == lastGap);

                lastGap = gapIndex;

                for (int i = 0; i < enemiesPerWave; i++)
                {
                    if (i == gapIndex || i == gapIndex + 1) continue;

                    float currentX = startX + (step * i);
                    Vector3 spawnPosition = ViewportToWorldPoint(currentX, offScreenOffset);
                    Vector3 direction = Vector3.back;

                    GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
                    Actor.Enemy.EnemyController controller = enemy.GetComponent<Actor.Enemy.EnemyController>();
                    ApplyRandomAttribute(controller);
                    controller.Launch(direction);
                }

                yield return new WaitForSeconds(1.2f);
            }
        }

        private IEnumerator SpawnArrowheadAssault(int waves)
        {
            float centerX = 0.5f;
            float topY = offScreenOffset;
            float spacingX = 0.1f;
            float spacingY = 0.1f;

            for (int w = 0; w < waves; w++)
            {
                int count = 3 + (w * 2);
                int midIndex = count / 2;

                for (int i = 0; i < count; i++)
                {
                    int offset = i - midIndex;
                    float currentX = centerX + (offset * spacingX);
                    float currentY = topY + (Mathf.Abs(offset) * spacingY);

                    Vector3 spawnPosition = ViewportToWorldPoint(currentX, currentY);
                    Vector3 direction = (target.localPosition - spawnPosition).normalized;

                    GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
                    Actor.Enemy.EnemyController controller = enemy.GetComponent<Actor.Enemy.EnemyController>();
                    ApplyRandomAttribute(controller);
                    controller.Launch(direction);

                    yield return new WaitForSeconds(0.05f);
                }

                yield return new WaitForSeconds(0.8f);
            }
        }

        private IEnumerator SpawnSweepingWave(int count)
        {
            float startX = 0f;
            float endX = 1f;
            float step = (endX - startX) / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float currentX = startX + (step * i);
                Vector3 spawnPosition = ViewportToWorldPoint(currentX, offScreenOffset);
                Vector3 direction = Vector3.back;

                GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
                Actor.Enemy.EnemyController controller = enemy.GetComponent<Actor.Enemy.EnemyController>();
                ApplyRandomAttribute(controller);
                controller.Launch(direction);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SpawnPincerAttack(int pairs)
        {
            for (int i = 0; i < pairs; i++)
            {
                Vector3 leftPos = ViewportToWorldPoint(0f, offScreenOffset);
                Vector3 rightPos = ViewportToWorldPoint(1f, offScreenOffset);

                Vector3 leftDir = (target.localPosition - leftPos).normalized;
                Vector3 rightDir = (target.localPosition - rightPos).normalized;

                GameObject leftEnemy = Instantiate(normalEnemyPrefab, leftPos, Quaternion.LookRotation(leftDir));
                Actor.Enemy.EnemyController leftController = leftEnemy.GetComponent<Actor.Enemy.EnemyController>();
                ApplyRandomAttribute(leftController);
                leftController.Launch(leftDir);

                GameObject rightEnemy = Instantiate(normalEnemyPrefab, rightPos, Quaternion.LookRotation(rightDir));
                Actor.Enemy.EnemyController rightController = rightEnemy.GetComponent<Actor.Enemy.EnemyController>();
                ApplyRandomAttribute(rightController);
                rightController.Launch(rightDir);

                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator SpawnMeteorRain(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float randomX = UnityEngine.Random.Range(0f, 1f);
                Vector3 spawnPosition = ViewportToWorldPoint(randomX, offScreenOffset);
                Vector3 direction = Vector3.back;

                GameObject enemy = Instantiate(normalEnemyPrefab, spawnPosition, Quaternion.LookRotation(direction));
                Actor.Enemy.EnemyController controller = enemy.GetComponent<Actor.Enemy.EnemyController>();
                ApplyRandomAttribute(controller);
                controller.Launch(direction);

                yield return new WaitForSeconds(0.15f);
            }
        }

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

        private Vector3 ViewportToWorldPoint(float vx, float vy)
        {
            if (mainCam == null) return new Vector3(0, fixedSpawnY, 0);

            Ray ray = mainCam.ViewportPointToRay(new Vector3(vx, vy, 0));
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, fixedSpawnY, 0));

            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return new Vector3(0, fixedSpawnY, 0);
        }
    }
}