using UnityEngine;
using UnityEngine.UI;

namespace UI.PrologueScene
{
    public class PrologueMenuPanel : MonoBehaviour
    {
        public Button tutorialButton;
        public Button backButton;

        private void Awake()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            if (tutorialButton != null)
            {
                tutorialButton.onClick.AddListener(() =>
                {
                    OnTutorialButtonClicked();
                });
            }

            if (backButton != null)
            {
                backButton.onClick.AddListener(() =>
                {
                    OnTitleButtonClicked();
                });
            }
        }

        private void OnTutorialButtonClicked()
        {
            Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.TutorialScene);
        }

        private void OnTitleButtonClicked()
        {
            Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.TitleScene);
        }
    }
}