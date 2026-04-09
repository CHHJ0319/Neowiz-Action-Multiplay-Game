using Actor.Player;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ActorManager : NetworkBehaviour
{
    public static ActorManager Instance { get; private set; }

    public GameObject[] playerPrefabs;

    private Dictionary<ulong, Actor.Player.PlayerController> players = new();
    private Dictionary<int, Actor.Enemy.EnemyController> enemies = new();

    private Vector3[] playerSpawnPositions =
    {
        //new Vector3(-2f, 0f, -2f),
        //new Vector3(2f, 0f, -2f),
        //new Vector3(-2f, 0f, -4f),
        //new Vector3(2f, 0f, -4f),
        new Vector3(-2f, 0f, -6f),
        new Vector3(2f, 0f, -6f),
        new Vector3(-2f, 0f, -8f),
        new Vector3(2f, 0f, -8f),
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
    public void SpawnPlayerServerRpc(ulong clientId, RpcParams rpcParams = default)
    {
        GameObject player = Instantiate(playerPrefabs[0]);
        player.transform.localPosition = playerSpawnPositions[players.Count];

        NetworkObject nv = player.GetComponent<NetworkObject>();
        nv.SpawnAsPlayerObject(clientId);

        players.Add(clientId, player.GetComponent<Actor.Player.PlayerController>());
    }

    public int GetPlayerCount()
    {
        return players.Count;
    }

    [Rpc(SendTo.Server)]
    public void SetPlayersRoleServerRpc(RpcParams rpcParams = default)
    {
        int[] roles = Services.RoleAssigner.AssignRandomRoles(ActorManager.Instance.GetPlayerCount()).ToArray();
        int[] colors = Services.RoleAssigner.AssignRandomColors(ActorManager.Instance.GetPlayerCount()).ToArray();

        int index = 0;
        int colorIndex = 0;
        Data.PlayerRole role;
        Data.ElementType color = Data.ElementType.Red;
        foreach (PlayerController player in players.Values)
        {
            if (roles[index] == 0)
            {
                role = Data.PlayerRole.Supporter;
            }
            else
            {
                role = Data.PlayerRole.Shooter;
                color = (Data.ElementType)colors[colorIndex];
                colorIndex++;
            }

            player.SetRole(role, color);
            index++;
        }
    }

    [Rpc(SendTo.Server)]
    public void SetPlayersCharacterServerRpc(RpcParams rpcParams = default)
    {
        RectTransform playerPlanels = UIManager.Instance.GetPlayerPanels();
        for (int i = 0; i< players.Count; i++)
        {
            UI.LobbyScene.PlayerPanel panel = playerPlanels.GetChild(i).GetComponent<UI.LobbyScene.PlayerPanel>();
            int characterIndex = panel.GetCharacterIndex();

            ulong key = (ulong)i;
            players[key].SetChareacter(characterIndex);
        }
    }

    public IEnumerator SpawnEnemyRow(Data.EnemyInfo[] enemyInfos, bool isTargeting)
    {
        Actor.Enemy.EnemySpawner.Instance.SpawnEnemyRow(enemyInfos, isTargeting);
        yield return null;
    }
}
