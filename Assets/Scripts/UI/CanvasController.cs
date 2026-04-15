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
        public UI.StageScene.StagePanel stagePanel;
        public UI.StageScene.PlayerStatusPanel playerStatusPanel;
        public UI.StageScene.TimerPanel timerPanel;
        public RectTransform pingPanel;

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
        }

        private void OnDisable()
        {
            Events.ActorEvents.OnPlayerFieldHPChanged -= UpdateBarricadeHPBar;
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
            if (playerPanels == null || playerPanels.childCount <= 0)
            {
                return;
            }

            foreach (RectTransform panel in playerPanels)
            {
                UI.LobbyScene.PlayerPanel playerPanel = panel.gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
                if(playerPanel.isDisabled.Value)
                {
                    int index = panel.GetSiblingIndex();
                    DataManager.Instance.SetClientInfo(index + 1);
                    DataManager.Instance.SetPlayerPanelIndex(index);

                    playerPanel.Initialize();
                    break;
                }
            }

            
        }

        public void DisablePlayerPanel(int index)
        {
            UI.LobbyScene.PlayerPanel playerPanel = playerPanels.GetChild(index).gameObject.GetComponent<UI.LobbyScene.PlayerPanel>();
            playerPanel.SetDisabledServerRpc(true);
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

            SetPlayerStatusPanel();
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
            StageManager.Instance.StartWaveServerRpc();
            //roundStartButton.gameObject.SetActive(false);
        }

        private void UpdateBarricadeHPBar(float hpRate)
        {
            if (barricadeHPBar == null) return;

            float currentHPRate = hpRate;
            currentHPRate = Mathf.Clamp(hpRate, 0f, 1f);

            barricadeHPBar.fillAmount = currentHPRate;
        }  

        private void SetPlayerStatusPanel()
        {
            playerStatusPanel.Initialize();
        }

        public void UpdateTimerPanel(float time, float timeRate)
        {
            if (timerPanel == null) return;

            timerPanel.UpdateTimerPanel(time, timeRate);
        }

        public void SetPingPanel(Data.PlayerRole[] roles, Data.ElementType[] types)
        {
            if (pingPanel == null) return;

            int index = 0;
            foreach(RectTransform item in pingPanel)
            {
                item.GetComponent<UI.StageScene.PingItem>().Initialize(roles[index], types[index]);
                index++;
                if (roles.Length <= index) break;
            }
        }
        #endregion
    }
}
