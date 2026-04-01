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
            gameStartButton.onClick.AddListener(() => OnGameStartButtonClicked(isHost));
            gameCancelButton.onClick.AddListener(() => OnGameCancelButtonClicked(isHost));
        }

        private void OnGameStartButtonClicked(bool isHost)
        {
        }

        private void OnGameCancelButtonClicked(bool isHost)
        {
        }
    }
}