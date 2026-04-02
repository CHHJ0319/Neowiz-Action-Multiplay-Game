using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class GameMenuPanel : MonoBehaviour
    {
        public Button gameStartButton;
        public Button gameCancelButton;

        public void Initialize(bool isHost)
        {
            TMP_Text buttonTitle = gameStartButton.GetComponentInChildren<TMP_Text>();
            if (isHost)
            {
                buttonTitle.text = "Start";
                gameStartButton.interactable = false;
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