using System;
using UnityEngine;

namespace Events
{
    public static class ActorEvents
    {
        public static event Action<Vector3, Vector3, Data.ElementType> OnSpawnNormalRequested;
        public static event Action<Vector3, Vector3, Data.ElementType, int> OnSpawnMultiLivesRequested;
        public static event Action<Vector3, Vector3, Data.ElementType> OnSpawnMultiTypeRequested;

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

        public static void SpawnNormalEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type)
        {
            OnSpawnNormalRequested?.Invoke(spawnPosition, direction, type);
        }
        public static void SpawnMultiLivesEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type, int lives)
        {
            OnSpawnMultiLivesRequested?.Invoke(spawnPosition, direction, type, lives);
        }
        public static void SpawnMultiTypeEnemy(Vector3 spawnPosition, Vector3 direction, Data.ElementType type)
        {
            OnSpawnMultiTypeRequested?.Invoke(spawnPosition, direction, type);
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