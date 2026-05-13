using UnityEngine;
using UnityEngine.UI;

namespace UI.TutorialScene
{
    public class HomeButton : MonoBehaviour
    {
        void Awake()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() => OnHomeButtonClicked());
        }

        private void OnHomeButtonClicked()
        {
            Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.TitleScene);
        }
    }
}