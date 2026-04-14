using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnSpawned;
        public static event Action<Data.PlayerRole, Data.ElementType> OnRoleAssigned;

        public static void Clear()
        {
            OnSpawned = null;
            OnRoleAssigned = null;
        }

        public static void SetPlayer()
        {
            OnSpawned?.Invoke();
        }

        public static void AssignRole(Data.PlayerRole role, Data.ElementType type)
        {
            OnRoleAssigned?.Invoke(role, type);
        }
    }
}