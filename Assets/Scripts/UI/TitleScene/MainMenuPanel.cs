using UnityEngine;
using UnityEngine.Diagnostics;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class MainMenuPanel : MonoBehaviour
    {
        [Header("Menu Buttons")]
        public Button tutorialButton;
        public Button createSessionButton;
        public Button joinSessionButton;
        public Button settingButton;
        public Button quitGameButton;

        private void Awake()
        {
            if (tutorialButton != null)
            {
                tutorialButton.onClick.AddListener(() => OnTutorialButtonClicked());
            }

            if (quitGameButton != null)
            {
                quitGameButton.onClick.AddListener(() => Events.GameEvents.QuitGame());
            }
        }

        private void OnTutorialButtonClicked()
        {
            Utils.SceneLoader.LoadSceneByName(Utils.SceneList.PrologueScene);
        }
    }
}