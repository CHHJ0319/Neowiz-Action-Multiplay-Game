using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnSpawned;

        public static void Clear()
        {
            OnSpawned = null;
        }

        public static void SetPlayer()
        {
            OnSpawned?.Invoke();
        }
    }
}