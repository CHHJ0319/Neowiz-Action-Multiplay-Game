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
        public Button roundStartButton;
        public Image barricadeHPBar;
        public UI.StageScene.ResultPanel resultPanel;
        public UI.StageScene.PlayerStatusPanel playerStatusPanel;

        [Header("LobbyScene")]
        public RectTransform playerPanels;
        public TextMeshProUGUI joinCode;
        public UI.LobbyScene.GameMenuPanel gameMenuPanel;

        private void Awake()
        {
            Instance = this;

            roundStartButton.onClick.AddListener(() => OnRoundStartButtonClicked());
        }

        private void OnEnable()
        {
            Events.ActorEvents.OnPlayerFieldHPChanged += UpdateBarricadeHPBar;
            Events.PlayerEvents.OnLobbySceneInitialized += SetPlayerPanel;
            Events.PlayerEvents.OnStageSceneInitialized += SetPlayerStatusPanel;
        }

        private void OnDisable()
        {
            Events.ActorEvents.OnPlayerFieldHPChanged -= UpdateBarricadeHPBar;
            Events.PlayerEvents.OnLobbySceneInitialized -= SetPlayerPanel;
            Events.PlayerEvents.OnStageSceneInitialized -= SetPlayerStatusPanel;
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

        public RectTransform GetPlayerPanels()
        {
            return playerPanels;
        }

        private void SetPlayerPanel(string playerName, int playerIndex, bool isOwner)
        {
            if (playerPanels == null || playerPanels.childCount <= 0) return;

            UI.LobbyScene.PlayerPanel playerPanel = playerPanels.GetChild(playerIndex).gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
            playerPanel.Initialize(playerName, isOwner);
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
            foreach (RectTransform child in playerPanels)
            {
                UI.LobbyScene.PlayerPanel panel = child.GetComponent<UI.LobbyScene.PlayerPanel>();
                if (panel.isReady.Value)
                {
                    readyCount++;
                }
            }

            return readyCount;
        }
        #endregion

        #region StageScene
        public void SetStageSceneSceneUI(bool isHost)
        {
            if(isHost)
            {
                if (roundStartButton != null)
                {
                    roundStartButton.gameObject.SetActive(true);
                    roundStartButton.onClick.AddListener(() => OnRoundStartButtonClicked());
                }
            }

            resultPanel.Initialize(isHost);
        }

        public void HidePointers()
        {
            foreach(RectTransform pointer in pointers)
            {
                pointer.gameObject.SetActive(false);
            }
        }

        public void ShowRoundStartButton()
        {
            roundStartButton.gameObject.SetActive(true);
        }

        public void SetResultPanelVisible(bool visivle)
        {
            resultPanel.gameObject.SetActive(visivle);
        }

        private void OnRoundStartButtonClicked()
        {
            RoundManager.Instance.StartRoundServerRpc();
            //roundStartButton.gameObject.SetActive(false);
        }

        private void UpdateBarricadeHPBar(float hpRate)
        {
            if (barricadeHPBar == null) return;

            float currentHPRate = hpRate;
            currentHPRate = Mathf.Clamp(hpRate, 0f, 1f);

            barricadeHPBar.fillAmount = currentHPRate;
        }  

        private void SetPlayerStatusPanel(Data.PlayerInfo info)
        {

            playerStatusPanel.Initialize(info);
        }
        #endregion
    }
}
