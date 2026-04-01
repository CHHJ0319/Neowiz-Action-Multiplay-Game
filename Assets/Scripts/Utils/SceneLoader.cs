using UnityEngine.SceneManagement;

namespace Utils
{
    public static class SceneLoader
    {
        public static void LoadSceneByName(SceneList scene)
        {
            string sceneName = scene.ToString();
            SceneManager.LoadScene(sceneName);
        }
    }
}