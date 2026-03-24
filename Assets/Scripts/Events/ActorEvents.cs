using System;

namespace Events
{
    public static class ActorEvents
    {
        public static event Action OnPlayerSpawned;

        public static void Clear()
        {
            OnPlayerSpawned = null;
        }

        public static void SetPlayer()
        {
            OnPlayerSpawned?.Invoke();
        }
    }
}