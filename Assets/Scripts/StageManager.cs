using Data;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class StageManager : NetworkBehaviour
{
    public static StageManager Instance { get; private set; }

    private float timePerWave = 180f;
    private float currentTime;

    private int waveIndex = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

        }
        else if (Instance != this)
        {
            Destroy(gameObject);
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

    private IEnumerator WavePreparationRoutine()
    {
        ActorManager.Instance.SetPlayersRoleServerRpc();
        UIManager.Instance.SetPingPanelClientRpc(ActorManager.Instance.GetAllPlayerRoles(), ActorManager.Instance.GetAllPlayerTypes());

        yield return new WaitForSeconds(5.0f);
    }

    private IEnumerator StartWave1()
    {
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
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(enemyInfos, false));

        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(enemyInfos, true));
        yield return new WaitForSeconds(0.5f);
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

    [Rpc(SendTo.Server)]
    public void UpdateWaveIndexServerRpc()
    {
        waveIndex++;
        UIManager.Instance.SetWaveTextClientRpc(waveIndex);
    }
}
