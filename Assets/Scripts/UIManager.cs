using UI;
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
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadComplete += OnSceneLoaded;
        }
    }

    public override void OnNetworkDespawn()
    {
        if (IsServer)
        {
            NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoaded;
        }
    }

    public void Initialize(int id, string sceneName)
    {
        if (sceneName == Utils.SceneList.LobbyScene.ToString())
        {
            SetJoinCode();
        }
    }

    public RectTransform GetPointer(int playerIndex)
    {
        return UI.CanvasController.Instance.GetPointer(playerIndex);
    }

    public void SetPlayerPanel(int playerIndex, bool isOwner)
    {
        UI.CanvasController.Instance.SetPlayerPanel(playerIndex, isOwner);
    }

    public void SetLobbyUI(bool isHost)
    {

    }

    private void SetJoinCode()
    {
        UI.CanvasController.Instance.SetJoinCode();
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadMode)
    {
        Initialize((int)clientId, sceneName);
    }
}
