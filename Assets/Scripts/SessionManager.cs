using Unity.Netcode;
using UnityEngine.SceneManagement;

public class SessionManager : NetworkBehaviour
{
    public static SessionManager Instance { get; private set; }

    public NetworkVariable<int> PlayerCount = new NetworkVariable<int>(0);

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
        PlayerCount.Value++;
    }

    [Rpc(SendTo.Server)]
    public void RemovePlayerServerRpc(RpcParams rpcParams = default)
    {
        PlayerCount.Value--;
    }

    [Rpc(SendTo.Server)]
    public void ResetPlayerCountServerRpc(RpcParams rpcParams = default)
    {
        PlayerCount.Value = 0;
    }

    public bool IsAllPlayersReady()
    {
        int readyCount = UIManager.Instance.GetReadyPlayerCount();

        if(PlayerCount.Value > 1 && PlayerCount.Value - 1 == readyCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
