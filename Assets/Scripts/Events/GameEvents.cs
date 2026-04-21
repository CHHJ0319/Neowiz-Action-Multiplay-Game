using System;

namespace Events
{
    public static class GameEvents
    {
        public static event Action OnQuitGame;
        public static event Action<string, string, string> OnStartHost;
        public static event Action<string> OnStartClient;
        public static event Action OnReadyGame;
        
        public static void Clear()
        {
            OnQuitGame = null;
            OnStartHost = null;
            OnStartClient = null;
            OnReadyGame = null;
        } 

        public static void StartHost(string playerName, string teamName, string password)
        {
            OnStartHost?.Invoke(playerName, teamName, password);
        }

        public static void StartClient(string joinCode)
        {
            OnStartClient?.Invoke(joinCode);
        }

        public static void QuitGame()
        {
            OnQuitGame?.Invoke();
        }

        public static void ReadyGame()
        {
            OnReadyGame?.Invoke();
        }
    }
}