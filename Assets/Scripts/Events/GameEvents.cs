using System;

namespace Events
{
    public static class GameEvents
    {
        public static event Action OnQuitButtonClicked;

        
        public static void QuitGame()
        {
            OnQuitButtonClicked?.Invoke();
        }
    }
}