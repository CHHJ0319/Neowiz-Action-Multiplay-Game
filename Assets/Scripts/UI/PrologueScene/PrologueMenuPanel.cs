using UnityEngine;
using UnityEngine.UI;

namespace UI.PrologueScene
{
    public class PrologueMenuPanel : MonoBehaviour
    {
        public Button tutorialSceneButton;
        public Button titleSceneButton;

        private void Awake()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            if (tutorialSceneButton != null)
            {
                tutorialSceneButton.onClick.AddListener(() =>
                {
                    OnTutorialButtonClicked();
                });
            }

            if (titleSceneButton != null)
            {
                titleSceneButton.onClick.AddListener(() =>
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