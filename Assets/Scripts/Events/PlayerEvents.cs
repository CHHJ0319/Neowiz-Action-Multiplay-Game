using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnPlayerSpawned;
        public static event Action<int, bool> OnPlayerLobbySceneInitialized;
        public static event Action OnPlayerStageSceneInitialized;
        public static void Clear()
        {
            OnPlayerSpawned = null;
            OnPlayerLobbySceneInitialized = null;
            OnPlayerStageSceneInitialized = null;
        }

        public static void SetPlayer()
        {
            OnPlayerSpawned?.Invoke();
        }

        public static void InitializePlayerInLobbyScene(int playerIndex, bool isOwner)
        {
            OnPlayerLobbySceneInitialized?.Invoke(playerIndex, isOwner);
        }

        public static void InitializePlayerInStageScene()
        {
            OnPlayerStageSceneInitialized?.Invoke();
        }
    }
}