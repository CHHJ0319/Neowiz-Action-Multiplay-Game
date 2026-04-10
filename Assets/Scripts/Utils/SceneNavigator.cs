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
                    SceneManager.LoadScene(sceneName);
                }
            }
            else
            {
                SceneManager.LoadScene(sceneName);
            }
        }

        public static string GetCurrentSceneName()
        {
            if (NetworkManager.Singleton != null && NetworkManager.Singleton.IsListening)
            {
                return SceneManager.GetActiveScene().name;
            }

            return SceneManager.GetActiveScene().name;
        }
    }
}