using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class MainMenuPanel : MonoBehaviour
    {
        public Image overlayBlocker;

        [Header("Menu Buttons")]
        public Button tutorialButton;
        public Button createSessionButton;
        public Button joinSessionButton;
        public Button settingButton;
        public Button quitGameButton;

        [Header("Popups")]
        public RectTransform popupPanels;
        public GameObject createSessionPanel;
        public GameObject joinSessionPanel;

        private bool isAnyPanelActive = false;

        private void Awake()
        {
            if (tutorialButton != null)
            {
                tutorialButton.onClick.AddListener(() => OnTutorialButtonClicked());
            }

            if (createSessionButton != null)
            {
                createSessionButton.onClick.AddListener(() => OnCreasteSessionButtonClicked());
            }

            if (joinSessionButton != null)
            {
                joinSessionButton.onClick.AddListener(() => OnJoinSessionButtonClicked());
            }

            if (quitGameButton != null)
            {
                quitGameButton.onClick.AddListener(() => Events.GameEvents.QuitGame());
            }
        }

        private void Update()
        {
            UpdateOverlayBlocker();
        }

        private void OnTutorialButtonClicked()
        {
            Utils.SceneLoader.LoadSceneByName(Utils.SceneList.PrologueScene);
        }

        private void UpdateOverlayBlocker()
        {
            isAnyPanelActive = false;
            for (int i = 0; i < popupPanels.childCount; i++)
            {
                if (popupPanels.GetChild(i).gameObject.activeSelf)
                {
                    isAnyPanelActive = true;
                    break;
                }
            }

            if (overlayBlocker.gameObject.activeSelf != isAnyPanelActive)
            {
                overlayBlocker.gameObject.SetActive(isAnyPanelActive);
            }
        }

        private void OnCreasteSessionButtonClicked()
        {
            if (createSessionPanel != null) createSessionPanel.SetActive(true);
        }

        private void OnJoinSessionButtonClicked()
        {
            if (joinSessionPanel != null) joinSessionPanel.SetActive(true);
        }
    }
}