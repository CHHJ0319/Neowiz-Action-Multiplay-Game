using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnPlayerSpawned;
        public static event Action OnPlayerStageSceneInitialized;
        public static void Clear()
        {
            OnPlayerSpawned = null;
            OnPlayerStageSceneInitialized = null;
        }

        public static void SetPlayer()
        {
            OnPlayerSpawned?.Invoke();
        }

        public static void InitializePlayerInStageScene()
        {
            OnPlayerStageSceneInitialized?.Invoke();
        }
    }
}