using Unity.Netcode;

public class SessionManager : NetworkBehaviour
{
    public static SessionManager Instance { get; private set; }

    private int playerCount = 0;

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

    [Rpc(SendTo.Server)]
    public void AddPlayerServerRpc(RpcParams rpcParams = default)
    {
        playerCount++;
    }

    public bool IsAllPlayersReady()
    {
        int readyCount = UIManager.Instance.GetReadyPlayerCount();

        if(playerCount > 1 && playerCount - 1 == readyCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
