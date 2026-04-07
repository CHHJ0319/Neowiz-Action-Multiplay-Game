using Actor;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class RoundManager : NetworkBehaviour
{
    public static RoundManager Instance { get; private set; }

    private Actor.Enemy.EnemyPattern pattern = new Actor.Enemy.EnemyPattern();

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
        StartPattern();
    }

    [Rpc(SendTo.Everyone)]
    public void StartRoundClientRpc()
    {
        //Events.RoundEvents.StartRound();
        StartPattern();
    }

    public void EndRound()
    {

    }

    private void StartPattern()
    {
        StartCoroutine(StartTutorialPattern());
    }

    private IEnumerator StartTutorialPattern()
    {
        Transform target = PlayerField.Instance.core;

        //Vector3 spawnPosition =
        //                Utils.ScreenSpaceConverter.ViewportToWorldPoint(0, 0.5f, 1.1f);
        //Vector3 direction = (target.position - spawnPosition).normalized;

        //Events.ActorEvents.SpawnNormalEnemy(spawnPosition, direction, Data.ElementType.Random);

        StartCoroutine(pattern.SpawnGapWall(target, 8, 1));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(pattern.SpawnArrowheadAssault(target, 1));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(pattern.SpawnSweepingWave(8));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(pattern.SpawnPincerAttack(target, 3));

        yield return new WaitForSeconds(1.5f);

        StartCoroutine(pattern.SpawnMeteorRain(10));

        yield return new WaitForSeconds(10.0f);

        Events.RoundEvents.EndRound();
        UIManager.Instance.EndRound();
    }
}
