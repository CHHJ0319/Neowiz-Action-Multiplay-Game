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
        public GameObject settingPanel;

        private bool isAnyPanelActive = false;

        private void Awake()
        {
            SetupButtons();
        }

        private void Update()
        {
            UpdateOverlayBlocker();
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

            if (createSessionButton != null)
            {
                createSessionButton.onClick.AddListener(() =>
                {
                    OnCreasteSessionButtonClicked();
                });
            }

            if (joinSessionButton != null)
            {
                joinSessionButton.onClick.AddListener(() => 
                { 
                    OnJoinSessionButtonClicked(); 
                });
            }

            if (settingButton != null)
            {
                settingButton.onClick.AddListener(() => 
                { 
                    OnSettingButtonClicked(); 
                });
            }

            if (quitGameButton != null)
            {
                quitGameButton.onClick.AddListener(() =>
                {
                    Events.GameEvents.QuitGame();
                });
            }
        }

        private void OnTutorialButtonClicked()
        {
            Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.PrologueScene);
        }

        private void OnCreasteSessionButtonClicked()
        {
            if (createSessionPanel != null) createSessionPanel.GetComponent<CreateSessionPanel>().SetVisible(true);
        }

        private void OnJoinSessionButtonClicked()
        {
            if (joinSessionPanel != null) joinSessionPanel.GetComponent<JoinSessionPanel>().SetVisible(true);
        }

        private void OnSettingButtonClicked()
        {
            if (settingPanel != null) settingPanel.GetComponent<SettingPanel>().SetVisible(true);
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
    }
}