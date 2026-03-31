using UI.LobbyScene;
using Unity.Netcode;
using UnityEngine;

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

    public RectTransform GetPointer(int playerIndex)
    {
        return UI.CanvasController.Instance.GetPointer(playerIndex);
    }

    public void SetPlayerPanel(int playerIndex, bool isOwner)
    {
        UI.CanvasController.Instance.SetPlayerPanel(playerIndex, isOwner);
    }
}
