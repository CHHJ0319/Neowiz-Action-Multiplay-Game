using UI.Common;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class CanvasController : MonoBehaviour
    {
        public static CanvasController Instance;

        [Header("Common")]
        public UI.Common.LeaderBoard leaderBoard;
        public UI.Common.TeamInfoPanel teamInfoPanel;

        [Header("TitleScene")]
        public RectTransform settingCofirmPanel;

        [Header ("StageScene")]
        public RectTransform pointers;
        public Button roundStartButton;
        public Image barricadeHPBar;
        public UI.StageScene.ResultPopup resultPopup;
        public UI.StageScene.StagePanel stagePanel;
        public UI.StageScene.PlayerStatusPanel playerStatusPanel;
        public UI.StageScene.TimerPanel timerPanel;
        public RectTransform pingPanel;
        public RectTransform playerRoleDisplay;

        [Header("LobbyScene")]
        public UI.LobbyScene.TeamNamePanel teamNamePanel;
        public UI.LobbyScene.JoinCodePanel joinCodePanel;
        public RectTransform characterSelectPanel;
        public UI.LobbyScene.LobbyMenuPanel LobbyMenuPanel;

        private void Awake()
        {
            Instance = this;
        }

        #region Common
        public void ShowLeaderBoard()
        {
            if (leaderBoard == null)
                return;

            leaderBoard.SetVisible(true);
        }

        public void ShowTeamInfoPanel()
        {
            if (teamInfoPanel == null)
                return;

            teamInfoPanel.SetVisible(true);
        }
        #endregion

        #region TitleScene
        #endregion

        #region LobbyScene
        public void SetLobbySceneUI(bool isHost)
        {
            if(isHost)
            {
                SetJoinCode();
            }

            LobbyMenuPanel.Initialize(isHost);
            SetTeamNamePanel();
            SetPlayerPanel();
        }

        private void SetTeamNamePanel()
        {
            if (teamNamePanel == null)
                return;

            string teamName = SessionManager.Instance.TeamName.Value.ToString();
            teamNamePanel.SetTeamName(teamName);
        }

        private void SetPlayerPanel()
        {
            if (characterSelectPanel == null || characterSelectPanel.childCount <= 0)
            {
                return;
            }

            foreach (RectTransform slot in characterSelectPanel)
            {
                UI.LobbyScene.PlayerSlot playerPanel = slot.gameObject.GetComponent<UI.LobbyScene.PlayerSlot>();
                if(playerPanel.isDisabled.Value)
                {
                    int index = slot.GetSiblingIndex();
                    DataManager.Instance.SetID(index);

                    playerPanel.Initialize();
                    break;
                }
            }

            
        }

        public void DisablePlayerPanel(int index)
        {
            UI.LobbyScene.PlayerSlot playerPanel = characterSelectPanel.GetChild(index).gameObject.GetComponent<UI.LobbyScene.PlayerSlot>();
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
            foreach (RectTransform child in characterSelectPanel)
            {
                UI.LobbyScene.PlayerSlot panel = child.GetComponent<UI.LobbyScene.PlayerSlot>();
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
            resultPopup.Initialize(isHost);
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

        public void SetResultPanelVisible(bool isVisible)
        {
            resultPopup.gameObject.SetActive(isVisible);
        }

        public void ShowResult(int startCount, string mvp)
        {
            resultPopup.ShowResult(startCount, mvp);
        }

        private void OnRoundStartButtonClicked()
        {
            StageManager.Instance.StartWaveServerRpc();
            roundStartButton.gameObject.SetActive(false);
        }

        public void UpdateBarricadeHPBar(float hpRate)
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

        public void UpdatePingMessage(int playerIndex, string message)
        {
            UI.StageScene.PingItem pingItem = pingPanel.GetChild(playerIndex).GetComponent<UI.StageScene.PingItem>();
            pingItem.UpdateRequestMessageText(message);
        }

        public UI.StageScene.Pointer GetPointer(int playerIndex)
        {
            if (pointers == null || pointers.childCount <= playerIndex) return null;

            return pointers.GetChild(playerIndex).GetComponent< UI.StageScene.Pointer>();
        }

        public void SetWaveText(int wave)
        {
            stagePanel.SetWaveText(wave);

        }

        public void SetPlayerRoleDisplay(Data.PlayerRole role, bool isActive)
        {
            if (playerRoleDisplay != null)
            {
                var image = playerRoleDisplay.GetChild((int)role);
                image.gameObject.SetActive(isActive);
            }
        }
        #endregion
    }
}
