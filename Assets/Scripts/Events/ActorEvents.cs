using System;
using UnityEngine;

namespace Events
{
    public static class ActorEvents
    {
        public static event Action OnPlayerSpawned;
        public static event Action<Vector3, Vector3> OnSpawnNormalRequested;
        public static event Action<Vector3, Vector3> OnSpawnMultiLivesRequested;
        public static event Action<Vector3, Vector3> OnSpawnMultiTypeRequested;

        public static void Clear()
        {
            OnPlayerSpawned = null;
            OnSpawnNormalRequested = null;
            OnSpawnMultiLivesRequested = null;
            OnSpawnMultiTypeRequested = null;
        }

        public static void SetPlayer()
        {
            OnPlayerSpawned?.Invoke();
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
    }
}