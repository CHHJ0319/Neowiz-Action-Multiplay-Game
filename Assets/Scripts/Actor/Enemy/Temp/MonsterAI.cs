using UnityEngine;

public class MonsterAI : MonoBehaviour
{
    [Header("이동 설정")]
    public float speed = 3f; // 몬스터가 쫓아오는 속도

    private Transform playerTarget;

    void Start()
    {
        // 1. 태그가 "Player"인 오브젝트를 찾아내서 목표물로 삼습니다!
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTarget = player.transform;
        }
    }

    void Update()
    {
        // 2. 플레이어가 맵에 살아있다면
        if (playerTarget != null)
        {
            // 플레이어의 위치를 향해 설정한 속도로 끊임없이 다가갑니다.
            transform.position = Vector3.MoveTowards(transform.position, playerTarget.position, speed * Time.deltaTime);

            // 몬스터가 플레이어를 쳐다보게 만듭니다. (바닥에 눕지 않게 Y축 높이는 유지)
            transform.LookAt(new Vector3(playerTarget.position.x, transform.position.y, playerTarget.position.z));
        }
    }
}