using Actor;
using System.Collections;
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

        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(6, false));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(5, false));

        yield return new WaitForSeconds(1.0f);

        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(6, true));
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(ActorManager.Instance.SpawnEnemyRow(5, true));

        //yield return new WaitForSeconds(8.0f);
    }
}
