using Data;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

public class StageManager : NetworkBehaviour
{
    public static StageManager Instance { get; private set; }

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
    public void StartRoundServerRpc(RpcParams rpcParams = default)
    {
        ActorManager.Instance.SetPlayersRoleServerRpc();
        //StartRoundClientRpc();
        StartPhases();
    }

    [Rpc(SendTo.Everyone)]
    public void StartRoundClientRpc()
    {
        //Events.RoundEvents.StartRound();
        StartPhases();
    }

    public void EndRound()
    {

    }

    private void StartPhases()
    {
        StartCoroutine(StartWave1());

        //Events.RoundEvents.EndRound();
        //UIManager.Instance.EndRound();
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

        //yield return new WaitForSeconds(8.0f);
    }
}
