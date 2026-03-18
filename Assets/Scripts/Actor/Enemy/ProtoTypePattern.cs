using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Actor.Enemy
{
    public class ProtoTypePattern : MonoBehaviour
    {
        public EnemySpawner spawner;

        void OnInteract(InputValue value)
        {
            if (value.isPressed)
            {
                StartCoroutine(SpawnAllPatternsRoutine());
            }
        }

        public IEnumerator SpawnAllPatternsRoutine()
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
                    Vector3 spawnPosition = ViewportToWorldPoint(currentX, spawner.offScreenOffset);
                    Vector3 direction = Vector3.back;

                    spawner.SpawnRandomEnemy(spawnPosition, direction);
                }

                yield return new WaitForSeconds(1.2f);
            }
        }

        private IEnumerator SpawnArrowheadAssault(int waves)
        {
            float centerX = 0.5f;
            float topY = spawner.offScreenOffset;
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
                  
                    spawner.SpawnRandomEnemy(spawnPosition);

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
                Vector3 spawnPosition = ViewportToWorldPoint(currentX, spawner.offScreenOffset);
                Vector3 direction = Vector3.back;

                spawner.SpawnRandomEnemy(spawnPosition);

                yield return new WaitForSeconds(0.1f);
            }
        }

        private IEnumerator SpawnPincerAttack(int pairs)
        {
            for (int i = 0; i < pairs; i++)
            {
                Vector3 leftPos = ViewportToWorldPoint(0f, spawner.offScreenOffset);
                Vector3 leftDir = (spawner.target.localPosition - leftPos).normalized;

                spawner.SpawnRandomEnemy(leftPos, leftDir);

                Vector3 rightPos = ViewportToWorldPoint(1f, spawner.offScreenOffset);
                Vector3 rightDir = (spawner.target.localPosition - rightPos).normalized;

                spawner.SpawnRandomEnemy(rightPos, rightDir);

                yield return new WaitForSeconds(0.3f);
            }
        }

        private IEnumerator SpawnMeteorRain(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float randomX = UnityEngine.Random.Range(0f, 1f);
                Vector3 spawnPosition = ViewportToWorldPoint(randomX, spawner.offScreenOffset);
                Vector3 direction = Vector3.back;

                spawner.SpawnRandomEnemy(spawnPosition, direction);

                yield return new WaitForSeconds(0.15f);
            }
        }

        private Vector3 ViewportToWorldPoint(float vx, float vy)
        {
            if (spawner.mainCam == null) return new Vector3(0, spawner.fixedSpawnY, 0);

            Ray ray = spawner.mainCam.ViewportPointToRay(new Vector3(vx, vy, 0));
            Plane groundPlane = new Plane(Vector3.up, new Vector3(0, spawner.fixedSpawnY, 0));

            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance);
            }

            return new Vector3(0, spawner.fixedSpawnY, 0);
        }
    }
}
