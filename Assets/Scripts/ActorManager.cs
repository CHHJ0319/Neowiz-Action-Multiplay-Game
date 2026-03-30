using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActorManager : NetworkBehaviour
{
    public static ActorManager Instance { get; private set; }

    public GameObject[] playerPrefabs;

    private Dictionary<ulong, Actor.Player.PlayerController> players = new();

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
        player.transform.localPosition = playerSpawnPositions[players.Count];
        player.GetComponent<Actor.Player.PlayerController>().Initialize(players.Count);

        NetworkObject nv = player.GetComponent<NetworkObject>();
        nv.SpawnAsPlayerObject(clientId);

        players.Add(clientId, player.GetComponent<Actor.Player.PlayerController>());
    }
}
