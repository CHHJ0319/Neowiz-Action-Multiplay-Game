using Unity.Netcode;
using UnityEngine;
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
            UI.CanvasController.Instance.SetLobbySceneUI(IsHost);
        }
        else if (sceneName == Utils.SceneList.TutorialScene.ToString())
        {
            UI.CanvasController.Instance.SetStageSceneSceneUI(IsHost);
        }
    }

    #region LobbyScene
    public int GetReadyPlayerCount()
    {
        return UI.CanvasController.Instance.GetReadyPlayerCount();
    }

    public RectTransform GetPlayerPanels()
    {
        return UI.CanvasController.Instance.GetPlayerPanels();
    }

    public void DisablePlayerPanel()
    {
        int index = DataManager.Instance.SessionPlayerIndex;
        UI.CanvasController.Instance.DisablePlayerPanel(index);
    }
    #endregion

    #region StageScene
    public void UpdateTimerPanel(float time, float timeRate)
    {
        UI.CanvasController.Instance.UpdateTimerPanel(time, timeRate);
    }

    public void SetPingPanel(Data.PlayerRole[] roles, Data.ElementType[] types)
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
    public void EndRoundClientRpc()
    {
        if(IsHost)
        {
            UI.CanvasController.Instance.ShowRoundStartButton();
        }
        UI.CanvasController.Instance.SetResultPanelVisible(true);
        UI.CanvasController.Instance.HidePointers();
    }

    [Rpc(SendTo.Everyone)]
    public void CloseResultPanelClientRpc()
    {
        UI.CanvasController.Instance.SetResultPanelVisible(false);
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
}
