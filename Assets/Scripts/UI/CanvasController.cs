using TMPro;
using UI.LobbyScene;
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
        public UI.LobbyScene.SessionMenuPanel sessionMenuPanel;
        public UI.LobbyScene.JoinCodePanel joinCodePanel;

        private void Awake()
        {
            Instance = this;
        }

        private void OnEnable()
        {
            Events.ActorEvents.OnPlayerFieldHPChanged += UpdateBarricadeHPBar;
            Events.PlayerEvents.OnStageSceneInitialized += SetPlayerStatusPanel;
        }

        private void OnDisable()
        {
            Events.ActorEvents.OnPlayerFieldHPChanged -= UpdateBarricadeHPBar;
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

            sessionMenuPanel.Initialize(isHost);
            SetPlayerPanel();
        }

        public RectTransform GetPlayerPanels()
        {
            return playerPanels;
        }

        private void SetPlayerPanel()
        {
            if (playerPanels == null || playerPanels.childCount <= 0) return;

            UI.LobbyScene.PlayerPanel playerPanel = playerPanels.GetChild(DataManager.Instance.ClientID).gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
            playerPanel.Initialize();
        }

        private void SetJoinCode()
        {
            if (joinCodePanel != null)
            {
                joinCodePanel.SetJoinCode();
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
