using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class LobbyMenuPanel : MonoBehaviour
    {
        public bool isTest = false;

        public Button confirmSessiontButton;
        public Button cancelSessionButton;

        private void Update()
        {
            if(Services.NetworkService.IsHost())
            {
                if(isTest)
                {
                    confirmSessiontButton.interactable = true;

                }
                else
                {
                    if (SessionManager.Instance.IsAllPlayersReady())
                    {
                        confirmSessiontButton.interactable = true;
                    }
                    else
                    {
                        confirmSessiontButton.interactable = false;
                    }
                }
            }
        }

        public void Initialize()
        {
            TMP_Text buttonTitle = confirmSessiontButton.GetComponentInChildren<TMP_Text>();
            if (Services.NetworkService.IsHost())
            {
                buttonTitle.text = "시작";
                confirmSessiontButton.interactable = false;
            }
            else
            {
                buttonTitle.text = "준비";
            }

            confirmSessiontButton.onClick.AddListener(() => OnConfirmSessionButtonClicked());
            cancelSessionButton.onClick.AddListener(() => OnCancelSessionButtonClicked());
        }

        private void OnConfirmSessionButtonClicked()
        {
            if(Services.NetworkService.IsHost())
            {
                SessionManager.Instance.SetMemberNames();
                Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.Stage1Scene);
            }
            else
            {
                Events.GameEvents.ReadyGame();
            }
        }

        private void OnCancelSessionButtonClicked()
        {
            GameManager.Instance.Disconnect();
        }
    }
}