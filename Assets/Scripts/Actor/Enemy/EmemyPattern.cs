using System.Collections;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyPattern
    {
        private static float fixedSpawnY = 0.5f;
        private static float offScreenOffset = 1.1f;

        public IEnumerator SpawnGapWall(Transform target, int enemiesPerWave, int waves)
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

                    Vector3 spawnPosition =
                        Utils.ScreenSpaceConverter.ViewportToWorldPoint(currentX, fixedSpawnY, offScreenOffset);
                    Vector3 direction = Vector3.back;

                    Events.ActorEvents.SpawnNormalEnemy(spawnPosition, direction);
                }

                yield return new WaitForSeconds(1.2f);
            }
        }

        public IEnumerator SpawnArrowheadAssault(Transform target, int waves)
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

                    Vector3 spawnPosition =
                        Utils.ScreenSpaceConverter.ViewportToWorldPoint(currentX, fixedSpawnY, currentY);
                    Vector3 direction = (target.localPosition - spawnPosition).normalized;

                    Events.ActorEvents.SpawnNormalEnemy(spawnPosition, direction);

                    yield return new WaitForSeconds(0.05f);
                }

                yield return new WaitForSeconds(0.8f);
            }
        }

        public IEnumerator SpawnSweepingWave(int count)
        {
            float startX = 0f;
            float endX = 1f;
            float step = (endX - startX) / (count - 1);

            for (int i = 0; i < count; i++)
            {
                float currentX = startX + (step * i);

                Vector3 spawnPosition =
                        Utils.ScreenSpaceConverter.ViewportToWorldPoint(currentX, fixedSpawnY, offScreenOffset);
                Vector3 direction = Vector3.back;

                Events.ActorEvents.SpawnNormalEnemy(spawnPosition, direction);

                yield return new WaitForSeconds(0.1f);
            }
        }

        public IEnumerator SpawnPincerAttack(Transform target, int pairs)
        {
            for (int i = 0; i < pairs; i++)
            {
                Vector3 leftPos =
                        Utils.ScreenSpaceConverter.ViewportToWorldPoint(0f, fixedSpawnY, offScreenOffset);
                Vector3 leftDir = (target.localPosition - leftPos).normalized;

                Events.ActorEvents.SpawnNormalEnemy(leftPos, leftDir);

                Vector3 rightPos =
                        Utils.ScreenSpaceConverter.ViewportToWorldPoint(1f, fixedSpawnY, offScreenOffset);
                Vector3 rightDir = (target.localPosition - rightPos).normalized;

                Events.ActorEvents.SpawnNormalEnemy(rightPos, rightDir);

                yield return new WaitForSeconds(0.3f);
            }
        }

        public IEnumerator SpawnMeteorRain(int count)
        {
            for (int i = 0; i < count; i++)
            {
                float randomX = Random.Range(0f, 1f);
                Vector3 spawnPosition =
                        Utils.ScreenSpaceConverter.ViewportToWorldPoint(randomX, fixedSpawnY, offScreenOffset);
                Vector3 direction = Vector3.back;

                Events.ActorEvents.SpawnNormalEnemy(spawnPosition, direction);

                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}
