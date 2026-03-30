using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager Instance { get; private set; }

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
    public void StartRoundServerRpc()
    {
        Data.PlayerType[] types = Services.RoleAssigner.AssignRandomRoles(ActorManager.Instance.GetPlayerCount());
        ActorManager.Instance.SetPlayersTypeServerRpc(types);
        StartRoundClientRpc();
    }

    [Rpc(SendTo.Everyone)]
    public void StartRoundClientRpc()
    {
        Events.RoundEvents.StartRound();
    }

    public void EndRound()
    {

    }
}
