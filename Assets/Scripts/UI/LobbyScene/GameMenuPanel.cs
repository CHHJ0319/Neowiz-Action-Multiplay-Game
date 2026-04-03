using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class GameMenuPanel : MonoBehaviour
    {
        public Button gameStartButton;
        public Button gameCancelButton;

        private bool _isHost = false;

        private void Update()
        {
            if(_isHost)
            {
                if(SessionManager.Instance.IsAllPlayersReady())
                {
                    gameStartButton.interactable = true;
                }
                else
                {
                    gameStartButton.interactable = false;
                }
            }
        }

        public void Initialize(bool isHost)
        {
            TMP_Text buttonTitle = gameStartButton.GetComponentInChildren<TMP_Text>();
            if (isHost)
            {
                buttonTitle.text = "Start";
                gameStartButton.interactable = false;

                _isHost = isHost; 
            }
            else
            {
                buttonTitle.text = "Ready";
            }

            gameStartButton.onClick.AddListener(() => OnGameStartButtonClicked(isHost));
            gameCancelButton.onClick.AddListener(() => OnGameCancelButtonClicked(isHost));
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
            if (isHost)
            {

            }
            else
            {

            }
        }
    }
}