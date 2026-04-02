using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;

        [Header ("StageScene")]
        public RectTransform pointers;

        [Header("LobbyScene")]
        public RectTransform playersPanel;
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
            if (playersPanel == null || playersPanel.childCount <= 0) return;

            UI.LobbyScene.PlayerPanel playerPanel = playersPanel.GetChild(playerIndex).gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
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
        #endregion
    }
}
