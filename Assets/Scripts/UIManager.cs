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

    public RectTransform GetPointer(int playerIndex)
    {
        return UI.CanvasController.Instance.GetPointer(playerIndex);
    }

    public int GetReadyPlayerCount()
    {
        return UI.CanvasController.Instance.GetReadyPlayerCount();
    }

    public void EndRound()
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

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadMode)
    {
        Initialize((int)clientId, sceneName);
    }

    public RectTransform GetPlayerPanels()
    {
        return UI.CanvasController.Instance.GetPlayerPanels();
    }
}
