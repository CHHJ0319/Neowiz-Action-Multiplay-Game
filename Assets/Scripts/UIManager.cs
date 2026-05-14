using System.Collections.Generic;
using UI.TitleScene;
using Unity.Netcode;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class UIManager : NetworkBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.SceneManager.OnLoadComplete += OnSceneLoaded;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoaded;
    }

    public void Initialize(int id, string sceneName)
    {
        if (sceneName == Utils.SceneList.LobbyScene.ToString())
        {
            UI.CanvasController.Instance.SetLobbySceneUI();
        }
        else if (sceneName == Utils.SceneList.Stage1Scene.ToString()
            || sceneName == Utils.SceneList.Stage2Scene.ToString())
        {
            UI.CanvasController.Instance.SetStageSceneSceneUI();
        }
    }

    #region Common
    public void ShowCommonPopup(string message)
    {
        UI.CanvasController.Instance.ShowCommonPopup(message);
    }

    public void SetupCommonPopup(UnityAction onConfirmAction)
    {
        UI.CanvasController.Instance.SetupCommonPopup(onConfirmAction);
    }
    #endregion

    #region Title Scene
    public void UpdateSettingPanel()
    {
        if (SettingPanel.Instance != null)
        {
            SettingPanel.Instance.UpdateUIState();
        }
    }
    #endregion

    #region LobbyScene
    public int GetReadyPlayerCount()
    {
        return UI.CanvasController.Instance.GetReadyPlayerCount();
    }

    public void DisablePlayerPanel()
    {
        int index = DataManager.Instance.ID;
        UI.CanvasController.Instance.DisablePlayerPanel(index);
    }

    public List<string> GetMemberNames()
    {
        return UI.CanvasController.Instance.GetMemberNames();
    }
    #endregion

    #region StageScene
    public void UpdateTimerPanel(float time, float timeRate)
    {
        UI.CanvasController.Instance.UpdateTimerPanel(time, timeRate);
    }

    [Rpc(SendTo.Everyone)]
    public void SetPingPanelClientRpc(Data.PlayerRole[] roles, Data.ElementType[] types)
    {
        UI.CanvasController.Instance.SetPingPanel(roles, types);
    }

    [Rpc(SendTo.Server)]
    public void UpdatePingMessageServerRpc(int playerIndex, string message, RpcParams rpcParams = default)
    {
        UpdatePingMessageClientRpc(playerIndex, message);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdatePingMessageClientRpc(int playerIndex, string message)
    {
        UI.CanvasController.Instance.UpdatePingMessage(playerIndex, message);
    }

    [Rpc(SendTo.Everyone)]
    public void SetPlayerRoleDisplayClientRpc(Data.PlayerRole[] roles, bool isActive)
    {
        Data.PlayerRole role = roles[DataManager.Instance.ID];
        UI.CanvasController.Instance.SetPlayerRoleDisplay(role, isActive);
    }

    [Rpc(SendTo.Everyone)]
    public void EndRoundClientRpc(int startCount, string mvp)
    {
        if(IsHost)
        {
            UI.CanvasController.Instance.ShowRoundStartButton();
        }
        UI.CanvasController.Instance.HidePointers();

        UI.CanvasController.Instance.SetResultPopupVisible(true);
        UI.CanvasController.Instance.ShowResult(startCount, mvp);
    }

    [Rpc(SendTo.Everyone)]
    public void CloseResultPopupClientRpc()
    {
        UI.CanvasController.Instance.SetResultPopupVisible(false);
    }

    [Rpc(SendTo.Everyone)]
    public void SetWaveTextClientRpc(int wave)
    {
        UI.CanvasController.Instance.SetWaveText(wave);
    }
    #endregion

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadMode)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            return;
        }
        
        Initialize((int)clientId, sceneName);
    }

    public UI.StageScene.Pointer GetPointer(int playerIndex)
    {
        return UI.CanvasController.Instance.GetPointer(playerIndex);
    }

    [Rpc(SendTo.Everyone)]
    public void UpdateBarricadeHPBarClientRpc(float hpRate)
    {
        UI.CanvasController.Instance.UpdateBarricadeHPBar(hpRate);
    }
}
