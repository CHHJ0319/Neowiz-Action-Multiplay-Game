using TMPro;
using UI.LobbyScene;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;

        [Header ("StageScene")]
        public RectTransform pointers;

        [Header("LobbyScene")]
        public RectTransform playerPanels;
        public TextMeshProUGUI joinCode;
        public UI.LobbyScene.GameMenuPanel gameMenuPanel;

        private void Awake()
        {
            Instance = this;
        }

        public RectTransform GetPointer(int playerIndex) 
        {
            if (pointers == null || pointers.childCount <= playerIndex) return null;

            //pointers.GetChild(playerIndex).gameObject.SetActive(true);
            return pointers.GetChild(playerIndex) as RectTransform;
        }

        #region LobbyScene
        public void SetLobbySceneUI(bool isHost)
        {
            if(isHost)
            {
                SetJoinCode();
            }
            gameMenuPanel.Initialize(isHost);
        }

        public void SetPlayerPanel(int playerIndex, bool isOwner)
        {
            if (playerPanels == null || playerPanels.childCount <= 0) return;

            UI.LobbyScene.PlayerPanel playerPanel = playerPanels.GetChild(playerIndex).gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
            playerPanel.Initialize(isOwner);
        }

        private void SetJoinCode()
        {
            if (joinCode != null)
            {
                joinCode.gameObject.SetActive(true);
                joinCode.text = Utils.NetworkService.JoinCode;
            }
        }

        public int GetReadyPlayerCount()
        {
            int readyCount = 0;
            foreach (PlayerPanel panel in playerPanels)
            {
                if(panel.isReady.Value)
                {
                    readyCount++;
                }
            }

            return readyCount;
        }
        #endregion
    }
}
