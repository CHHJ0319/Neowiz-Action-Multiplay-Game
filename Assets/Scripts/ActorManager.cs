using Actor;
using Actor.Player;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class ActorManager : NetworkBehaviour
{
    public static ActorManager Instance { get; private set; }

    public GameObject[] playerPrefabs;

    public NetworkVariable<int> playerCount = new NetworkVariable<int>(); 

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

    public IEnumerator SpawnPlayer(ulong clientId)
    {
        SpawnPlayerServerRpc(clientId);
        yield return null;
    }

    [Rpc(SendTo.Server)]
    public void SpawnPlayerServerRpc(ulong clientId)
    {
        GameObject player = Instantiate(playerPrefabs[0]);

        NetworkObject nv = player.GetComponent<NetworkObject>();
        nv.SpawnAsPlayerObject(clientId);

        player.GetComponent<PlayerController>().SetPointer(playerCount.Value);
        Vector3 randomPos = new Vector3(0f, 0f, -3f);
        player.GetComponent<PlayerController>().SetPosition(randomPos);

        playerCount.Value++;
    }
}
