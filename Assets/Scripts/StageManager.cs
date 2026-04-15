using Data;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class StageManager : NetworkBehaviour
{
    public static StageManager Instance { get; private set; }

    private float timePerWave = 180f;
    private float currentTime;

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
        currentTime = timePerWave;
        StartCoroutine(StartTimer());

        ActorManager.Instance.SetPlayersRoleServerRpc();
        StartWaveClientRpc(ActorManager.Instance.GetAllPlayerRoles(), ActorManager.Instance.GetAllPlayerTypes());
        StartCoroutine(StartWave1());
    }

    [Rpc(SendTo.Everyone)]
    public void StartWaveClientRpc(Data.PlayerRole[] roles, Data.ElementType[] types)
    {
        UIManager.Instance.SetPingPanel(roles, types);
    }

    public void EndRound()
    {

    }

    private IEnumerator StartWave1()
    {
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

        //Events.RoundEvents.EndRound();
        //UIManager.Instance.EndRound();
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
}
