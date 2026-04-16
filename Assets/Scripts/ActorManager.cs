using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ActorManager : NetworkBehaviour
{
    public static ActorManager Instance { get; private set; }

    public bool isTest = false;

    public GameObject[] playerPrefabs;

    private Dictionary<ulong, Actor.Player.PlayerController> players = new();
    private Dictionary<ulong, Actor.Enemy.EnemyController> enemies = new();

    private Vector3[] playerSpawnPositions =
    {
        //new Vector3(-2f, 0f, -2f),
        //new Vector3(2f, 0f, -2f),
        //new Vector3(-2f, 0f, -4f),
        //new Vector3(2f, 0f, -4f),
        new Vector3(-2f, 1f, -6f),
        new Vector3(2f, 1f, -6f),
        new Vector3(-2f, 1f, -8f),
        new Vector3(2f, 1f, -8f),
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

    public override void OnNetworkSpawn()
    {
        NetworkManager.SceneManager.OnLoadComplete += OnSceneLoaded;
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.SceneManager.OnLoadComplete -= OnSceneLoaded;
    }

    public void Initialize(int id, string sceneName)
    {
        if (sceneName == Utils.SceneList.LobbyScene.ToString())
        {

        }
        else if (sceneName == Utils.SceneList.TutorialScene.ToString())
        {
            SpawnPlayerServerRpc(NetworkManager.Singleton.LocalClientId, DataManager.Instance.CharacterIndex, DataManager.Instance.SessionPlayerIndex);
        }
    }

    #region Player
    [Rpc(SendTo.Server)]
    private void SpawnPlayerServerRpc(ulong clientId, int characterIndex, int spawnIdex, RpcParams rpcParams = default)
    {
        GameObject player = Instantiate(playerPrefabs[characterIndex]);
        player.transform.localPosition = playerSpawnPositions[spawnIdex];

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
        foreach (Actor.Player.PlayerController player in players.Values)
        {
            if (roles[index] == 0)
            {
                if (isTest)
                {
                    role = Data.PlayerRole.Shooter;
                    color = Data.ElementType.Red;
                }
                else
                {
                    role = Data.PlayerRole.Supporter;
                }
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

    public Data.PlayerRole[] GetAllPlayerRoles()
    {
        Data.PlayerRole[] roles = new Data.PlayerRole[players.Count];
        foreach (Actor.Player.PlayerController player in players.Values)
        {
            int index = player.playerIndex.Value;
            roles[index] = (Data.PlayerRole)player.Role.Value;
        }
        return roles;
    }

    public Data.ElementType[] GetAllPlayerTypes()
    {
        Data.ElementType[] types = new Data.ElementType[players.Count];
        foreach (Actor.Player.PlayerController player in players.Values)
        {
            int index = player.playerIndex.Value;
            types[index] = (Data.ElementType)player.Type.Value;
        }
        return types;
    }
    #endregion

    #region Enemy
    public IEnumerator SpawnEnemyRow(Data.EnemyInfo[] enemyInfos, bool isTargeting)
    {
        Actor.Enemy.EnemySpawner.Instance.SpawnEnemyRow(enemyInfos, isTargeting);
        yield return null;
    }

    public void AddEnemy(ulong id, Actor.Enemy.EnemyController enemy)
    {
        enemies.Add(id, enemy);
    }

    public void RemoveEnemy(ulong id)
    {
        if (enemies.ContainsKey(id))
        {
            enemies.Remove(id);
        }
        else
        {
            Debug.LogWarning($"[Server] 제거하려는 ID({id})를 찾을 수 없습니다.");
        }

        if (enemies.Count == 0)
        {
            StartCoroutine(StageManager.Instance.EndWave());
        }
    }

    [Rpc(SendTo.Server)]
    public void ClearEemiesServerRpc(RpcParams rpcParams = default)
    {
        var enemyList = enemies.Values.ToList();

        foreach (var enemy in enemyList)
        {
            if (enemy != null)
            {
                enemy.DespawnSelf();
            }
        }

        enemies.Clear();
    }
    #endregion

    public float GetPlayerFiledHPRate()
    {
        return Actor.PlayerField.Instance.GetPlayerFiledHPRate();
    }

    [Rpc(SendTo.Everyone)]
    public void ResetPlayerFiledHPClientRpc()
    {
        Actor.PlayerField.Instance.ResetPlayerFiledHP();
    }

    private void OnSceneLoaded(ulong clientId, string sceneName, LoadSceneMode loadMode)
    {
        if (clientId != NetworkManager.Singleton.LocalClientId)
        {
            return;
        }

        Initialize((int)clientId, sceneName);
    }
}
