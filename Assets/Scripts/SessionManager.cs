using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;

public class SessionManager : NetworkBehaviour
{
    public static SessionManager Instance { get; private set; }

    public NetworkVariable<FixedString64Bytes> TeamName = new NetworkVariable<FixedString64Bytes>();
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

    public void Initialize(string teamName)
    {
        SetTeamNameServerRpc(teamName);
        ClearServerRpc();
        AddPlayerServerRpc();
    }

    [Rpc(SendTo.Server)]
    public void SetTeamNameServerRpc(string teamName, RpcParams rpcParams = default)
    {
        TeamName.Value = teamName;
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
    public void ClearServerRpc(RpcParams rpcParams = default)
    {
        PlayerCount.Value = 0;
    }

    public bool IsAllPlayersReady()
    {
        int readyCount = UIManager.Instance.GetReadyPlayerCount();

        if(PlayerCount.Value > 3 && 
            PlayerCount.Value - 1 == readyCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
