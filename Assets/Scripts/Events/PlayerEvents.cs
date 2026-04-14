using System;

namespace Events
{
    public static class PlayerEvents
    {
        public static event Action OnSpawned;
        public static event Action<Data.PlayerRole, Data.ElementType> OnRoleAssigned;
        public static event Action<int> OnAmmoChanged;

        public static void Clear()
        {
            OnSpawned = null;
            OnRoleAssigned = null;
            OnAmmoChanged = null;
        }

        public static void SetPlayer()
        {
            OnSpawned?.Invoke();
        }

        public static void UpdateRoleUI(Data.PlayerRole role, Data.ElementType type)
        {
            OnRoleAssigned?.Invoke(role, type);
        }

        public static void UpdateAmmoUI(int ammo)
        {
            OnAmmoChanged?.Invoke(ammo);
        }
    }
}