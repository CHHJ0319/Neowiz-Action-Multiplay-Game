using Data;
using System.Collections;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : NetworkBehaviour
{
    public static StageManager Instance { get; private set; }

    private float timePerWave = 180f;
    private float currentTime;

    public int WaveIndex { get; private set; } = 1;
    private bool isWaveRunning = false;

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

    private void Initialize(int id, string sceneName)
    {
        if (sceneName == Utils.SceneList.LobbyScene.ToString())
        {

        }
        else if (sceneName == Utils.SceneList.TutorialScene.ToString()
            || sceneName == Utils.SceneList.Stage1Scene.ToString()
            || sceneName == Utils.SceneList.Stage2Scene.ToString())
        {
            WaveIndex = 1;
        }
    }

    [Rpc(SendTo.Server)]
    public void StartWaveServerRpc(RpcParams rpcParams = default)
    {
        StartCoroutine(WaveRoutine());
    }

    private IEnumerator WaveRoutine()
    {
        yield return StartCoroutine(WavePreparationRoutine());

        yield return StartCoroutine(StartWave1());
        yield return null;
    }

    public IEnumerator EndWave()
    {
        isWaveRunning = false;
        ActorManager.Instance.ClearItemsServerRpc();

        int startCount = EvaluateWave();
        string mvp = SessionManager.Instance.GetMVP();
        UIManager.Instance.EndRoundClientRpc(startCount, mvp);

        SessionManager.Instance.CalculateTotalScore();
        yield return null;
    }

    private IEnumerator WavePreparationRoutine()
    {
        ActorManager.Instance.SetPlayersRoleServerRpc();
        UIManager.Instance.SetPingPanelClientRpc(ActorManager.Instance.GetAllPlayerRoles(), ActorManager.Instance.GetAllPlayerTypes());
        UIManager.Instance.SetPlayerRoleDisplayClientRpc(ActorManager.Instance.GetAllPlayerRoles(), true);
        yield return new WaitForSeconds(3.0f);

        UIManager.Instance.SetPlayerRoleDisplayClientRpc(ActorManager.Instance.GetAllPlayerRoles(), false);
        yield return new WaitForSeconds(2.0f);
    }

    private int EvaluateWave()
    {
        int starCount = 0;
        float hp = ActorManager.Instance.GetPlayerFiledHPRate();
        if(hp <= 0)
        {
            starCount = 0;
        }
        else if(hp <= 0.5)
        {
            starCount = 1;
        }
        else if(hp <= 0.8)
        {
            starCount = 2;
        }
        else
        {
            starCount = 3;
        }

        return starCount;
    }

    private IEnumerator StartWave1()
    {
        isWaveRunning = true;

        StartCoroutine(SpawnItemPeriodicallyRoutine());

        currentTime = timePerWave;
        StartCoroutine(StartTimer());

        Data.EnemyInfo[] enemyInfos = new Data.EnemyInfo[]
        {
            new Data.EnemyInfo { type = EnemyType.Single, lives = 1 },
            new Data.EnemyInfo { type = EnemyType.Single, lives = 1 },
            new Data.EnemyInfo { type = EnemyType.Single, lives = 1 },
            new Data.EnemyInfo { type = EnemyType.Single, lives = 1 },
            new Data.EnemyInfo { type = EnemyType.Single, lives = 1 },
            new Data.EnemyInfo { type = EnemyType.Single, lives = 1 },
        };
        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(enemyInfos, false));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(enemyInfos, false));

        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(enemyInfos, true));
        yield return new WaitForSeconds(1.0f);
        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(enemyInfos, true));
    }

    private IEnumerator StartTimer()
    {
        while (currentTime > 0)
        {
            currentTime -= Time.deltaTime;

            UIManager.Instance.UpdateTimerPanel(currentTime, currentTime / timePerWave);
            yield return null;
        }

        currentTime = 0;
        UIManager.Instance.UpdateTimerPanel(0, 0);
    }

    private IEnumerator SpawnItemPeriodicallyRoutine()
    {
        while (isWaveRunning)
        {
            ActorManager.Instance.SpawnItemServerRpc();

            yield return new WaitForSeconds(5.0f);
        }
    }

    [Rpc(SendTo.Server)]
    public void UpdateWaveIndexServerRpc(RpcParams rpcParams = default)
    {
        WaveIndex++;
        UIManager.Instance.SetWaveTextClientRpc(WaveIndex);
    }

    [Rpc(SendTo.Server)]
    public void ResetStageServerRpc(RpcParams rpcParams = default)
    {
        WaveIndex = 1;
        UIManager.Instance.SetWaveTextClientRpc(WaveIndex);
        ActorManager.Instance.ResetPlayerFiledHPClientRpc();
        SessionManager.Instance.ResetTotalScore();
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
