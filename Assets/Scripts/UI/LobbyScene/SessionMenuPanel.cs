using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class SessionMenuPanel : MonoBehaviour
    {
        public Button confirmSessiontButton;
        public Button cancelSessionButton;

        private bool _isHost = false;

        private void Update()
        {
            if(_isHost)
            {
                if(SessionManager.Instance.IsAllPlayersReady())
                {
                    confirmSessiontButton.interactable = true;
                }
                else
                {
                    confirmSessiontButton.interactable = false;
                }
            }
        }

        public void Initialize(bool isHost)
        {
            TMP_Text buttonTitle = confirmSessiontButton.GetComponentInChildren<TMP_Text>();
            if (isHost)
            {
                buttonTitle.text = "시작";
                confirmSessiontButton.interactable = false;

                _isHost = isHost; 
            }
            else
            {
                buttonTitle.text = "준비";
            }

            confirmSessiontButton.onClick.AddListener(() => OnGameStartButtonClicked(isHost));
            cancelSessionButton.onClick.AddListener(() => OnGameCancelButtonClicked(isHost));
        }

        private void OnGameStartButtonClicked(bool isHost)
        {
            if(isHost)
            {
                ActorManager.Instance.SetPlayersCharacterServerRpc();
                Utils.SceneNavigator.LoadSceneByName(Utils.SceneList.TutorialScene);
            }
            else
            {
                Events.GameEvents.ReadyGame();
            }
        }

        private void OnGameCancelButtonClicked(bool isHost)
        {
            GameManager.Instance.Disconnect(isHost);
        }
    }
}