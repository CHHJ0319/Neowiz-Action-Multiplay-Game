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

    private Vector3[] playerSpawnPositions =
    {
        new Vector3(-2f, 0f, -2f),
        new Vector3(2f, 0f, -2f),
        new Vector3(-2f, 0f, -4f),
        new Vector3(2f, 0f, -4f),
    };

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
        player.transform.localPosition = playerSpawnPositions[playerCount.Value];
        player.GetComponent<Actor.Player.PlayerController>().Initialize(playerCount.Value);

        NetworkObject nv = player.GetComponent<NetworkObject>();
        nv.SpawnAsPlayerObject(clientId);

        playerCount.Value++;
    }
}
