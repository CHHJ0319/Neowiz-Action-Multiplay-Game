using System;
using UnityEngine;

namespace Events
{
    public static class ActorEvents
    {
        public static event Action<Vector3, Vector3> OnSpawnNormalRequested;
        public static event Action<Vector3, Vector3> OnSpawnMultiLivesRequested;
        public static event Action<Vector3, Vector3> OnSpawnMultiTypeRequested;

        public static event Action<float> OnPlayerFieldHPChanged;
        public static event Action<float> OnEnemyEnteredPlayerField;

        public static void Clear()
        {
            OnSpawnNormalRequested = null;
            OnSpawnMultiLivesRequested = null;
            OnSpawnMultiTypeRequested = null;

            OnPlayerFieldHPChanged = null;
            OnEnemyEnteredPlayerField = null;
        }

        public static void SpawnNormalEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            OnSpawnNormalRequested?.Invoke(spawnPosition, direction);
        }
        public static void SpawnMultiLivesEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            OnSpawnMultiLivesRequested?.Invoke(spawnPosition, direction);
        }
        public static void SpawnMultiTypeEnemy(Vector3 spawnPosition, Vector3 direction)
        {
            OnSpawnMultiTypeRequested?.Invoke(spawnPosition, direction);
        }

        public static void UpdatePlayerFieldHPBar(float hpRate)
        {
            OnPlayerFieldHPChanged?.Invoke(hpRate);
        }

        public static void HandleEnemyPlayerFieldCollision(float damage)
        {
            OnEnemyEnteredPlayerField?.Invoke(damage);
        }
    }
}