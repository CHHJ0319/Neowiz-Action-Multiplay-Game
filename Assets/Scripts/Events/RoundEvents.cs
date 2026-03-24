using System;
using UnityEngine;

namespace Events
{
    public static class RoundEvents
    {
        public static event Action OnRoundEnded;
        public static event Action OnRoundStarted;

        public static void Clear()
        {
            OnRoundEnded = null;
            OnRoundStarted = null;
        }

        public static void EndRound()
        {
            OnRoundEnded?.Invoke();
        }

        public static void StartRound()
        {
            OnRoundStarted?.Invoke();
        }
    }
}
