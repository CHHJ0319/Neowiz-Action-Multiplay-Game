using System;
using UnityEngine;

namespace Events
{
    public static class ActorEvents
    {
        public static event Action<float> OnPlayerFieldHPChanged;
        public static event Action<float> OnEnemyEnteredPlayerField;

        public static void Clear()
        {
            OnPlayerFieldHPChanged = null;
            OnEnemyEnteredPlayerField = null;
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