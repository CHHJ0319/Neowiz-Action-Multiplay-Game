using System;
using UnityEngine;

namespace Events
{
    public static class RoundEvents
    {
        public static event Action OnRoundEnded;

        public static void EndRound()
        {
            OnRoundEnded?.Invoke();
        }
    }
}
