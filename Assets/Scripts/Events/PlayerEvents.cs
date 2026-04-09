using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnSpawned;
        public static event Action<Data.PlayerInfo> OnStageSceneInitialized;
        public static event Action<Data.PlayerInfo> OnRoleAssigned;
        public static void Clear()
        {
            OnSpawned = null;
            OnStageSceneInitialized = null;
            OnRoleAssigned = null;
        }

        public static void SetPlayer()
        {
            OnSpawned?.Invoke();
        }

        public static void InitializeInStageScene(Data.PlayerInfo info)
        {
            OnStageSceneInitialized?.Invoke(info);
        }

        public static void AssignPlayerRole(Data.PlayerInfo info)
        {
            OnRoleAssigned?.Invoke(info);
        }
    }
}