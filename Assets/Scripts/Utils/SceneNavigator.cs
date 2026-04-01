using Unity.Netcode;
using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneNavigator
    {
        public static void LoadSceneByName(SceneList scene)
        {
            string sceneName = scene.ToString();
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                if (NetworkManager.Singleton.IsServer)
                {
                    NetworkManager.Singleton.SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
                }
                else
                {
                    UnityEngine.Debug.LogWarning("Clients cannot initiate network scene changes.");
                }
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }
    }
}