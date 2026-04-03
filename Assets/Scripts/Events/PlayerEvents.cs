using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnPlayerSpawned;
        public static event Action<string, int, bool> OnPlayerLobbySceneInitialized;
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

        public static void InitializePlayerInLobbyScene(string playerName, int playerIndex, bool isOwner)
        {
            OnPlayerLobbySceneInitialized?.Invoke(playerName, playerIndex, isOwner);
        }

        public static void InitializePlayerInStageScene()
        {
            OnPlayerStageSceneInitialized?.Invoke();
        }
    }
}