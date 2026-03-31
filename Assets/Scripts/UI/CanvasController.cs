using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;

        public RectTransform pointers;
        public RectTransform playerPanels;

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

        public void SetPlayerPanel(int playerIndex, bool isOwner)
        {
            if (playerPanels == null || playerPanels.childCount <= 0) return;

            UI.LobbyScene.PlayerPanel playerPanel = playerPanels.GetChild(playerIndex).gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
            playerPanel.Initialize(isOwner);
        }
    }
}
