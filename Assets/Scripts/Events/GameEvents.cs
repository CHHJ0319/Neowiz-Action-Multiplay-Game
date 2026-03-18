using System;

namespace Events
{
    public static class GameEvents
    {
        public static event Action OnQuitGame;

        
        public static void QuitGame()
        {
            OnQuitGame?.Invoke();
        }
    }
}