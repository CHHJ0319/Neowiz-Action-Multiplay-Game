using UnityEngine;

public class MonsterSpawner : MonoBehaviour
{
    [Header("스폰 설정")]
    // 🎯 한 종류가 아니라, 여러 종류의 몬스터를 담을 수 있는 '배열(Array)' 칸으로 바꿨습니다!
    public GameObject[] monsterPrefabs;
    public float spawnInterval = 3f; // 3초마다 소환

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            SpawnRandomMonster();
            timer = 0f;
        }
    }

    void SpawnRandomMonster()
    {
        // 유니티 창에 몬스터 종류가 하나라도 등록되어 있다면
        if (monsterPrefabs.Length > 0)
        {
            // 🎯 0번 칸부터 등록된 몬스터 종류의 끝 번호 중에서 '랜덤'으로 하나를 뽑습니다.
            int randomIndex = Random.Range(0, monsterPrefabs.Length);

            // 랜덤으로 당첨된 그 몬스터를 스폰 지역에 생성!
            Instantiate(monsterPrefabs[randomIndex], transform.position, Quaternion.identity);
        }
    }
}