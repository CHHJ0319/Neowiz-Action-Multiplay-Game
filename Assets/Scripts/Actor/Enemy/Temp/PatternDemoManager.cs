using System.Collections;
using UnityEngine;

namespace Actor.Enemy
{
    public class PatternDemoManager : MonoBehaviour
    {
        [Header("1. 몬스터 4마리 (Size 4)")]
        public GameObject[] monsterPrefabs;

        [Header("2. 구역(큐브) 설정")]
        public GameObject spawnArea;
        public GameObject pathArea;
        public GameObject targetArea;

        [Header("3. 이동 속도")]
        public float moveSpeed = 7f;

        void Update()
        {
            // 🎯 [수정됨] 유니티 단축키 충돌을 막기 위해 'Shift(쉬프트)' 키로 바꿨습니다!
            bool isShift = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

            // 1번: 일직선 (기본 1줄 / Shift 10줄)
            if (Input.GetKeyDown(KeyCode.Alpha1)) StartCoroutine(Pattern1_Horizontal(isShift ? 10 : 1));
            // 2번: 세로줄 (기본 1줄 / Shift 5줄)
            if (Input.GetKeyDown(KeyCode.Alpha2)) StartCoroutine(Pattern2_Vertical(isShift ? 5 : 1));
            // 3번: V자 (기본 1줄 / Shift 5줄)
            if (Input.GetKeyDown(KeyCode.Alpha3)) StartCoroutine(Pattern3_V(isShift ? 5 : 1));
            // 4번: 역V자 (기본 1줄 / Shift 5줄)
            if (Input.GetKeyDown(KeyCode.Alpha4)) StartCoroutine(Pattern4_InvertedV(isShift ? 5 : 1));
            // 5번: X자 (기본 1줄 / Shift 3줄)
            if (Input.GetKeyDown(KeyCode.Alpha5)) StartCoroutine(Pattern5_X(isShift ? 3 : 1));
            // 6번: 세로 지그재그 (넓고 몬스터 많음)
            if (Input.GetKeyDown(KeyCode.Alpha6)) StartCoroutine(Pattern6_WideZigzag());
            // 7번: 무작위 폭격
            if (Input.GetKeyDown(KeyCode.Alpha7)) StartCoroutine(Pattern7_Random());
            // 8번: 공중 낙하 (바닥에 닿아야 이동 시작)
            if (Input.GetKeyDown(KeyCode.Alpha8)) StartCoroutine(Pattern8_AirDrop());
        }

        private void SpawnRandomMonster(Vector3 pos, Vector3 target, bool isAirDrop = false)
        {
            if (monsterPrefabs == null || monsterPrefabs.Length == 0) return;
            GameObject selected = monsterPrefabs[Random.Range(0, monsterPrefabs.Length)];

            Vector3 straightTarget = new Vector3(pos.x, target.y, target.z);
            Vector3 dir = straightTarget - pos;
            dir.y = 0;
            dir = dir.normalized;

            if (dir == Vector3.zero)
                dir = (targetArea.transform.position.z < spawnArea.transform.position.z) ? Vector3.back : Vector3.forward;

            Vector3 spawnPos = isAirDrop ? new Vector3(pos.x, pos.y + 15f, pos.z) : pos;

            GameObject go = Instantiate(selected, spawnPos, Quaternion.LookRotation(dir));

            SimpleMove move = go.GetComponent<SimpleMove>();
            if (move == null) move = go.AddComponent<SimpleMove>();

            move.Initialize(dir, moveSpeed, isAirDrop, pos.y);
        }

        private Vector3 GetPosByRatio(GameObject areaObj, float xRatio)
        {
            if (areaObj == null || areaObj.GetComponent<Renderer>() == null) return transform.position;
            Bounds b = areaObj.GetComponent<Renderer>().bounds;
            return new Vector3(Mathf.Lerp(b.min.x, b.max.x, xRatio), areaObj.transform.position.y, areaObj.transform.position.z);
        }
        private Vector3 GetRandomPosInArea(GameObject areaObj)
        {
            if (areaObj == null || areaObj.GetComponent<Renderer>() == null) return transform.position;
            Bounds b = areaObj.GetComponent<Renderer>().bounds;
            return new Vector3(Random.Range(b.min.x, b.max.x), areaObj.transform.position.y, Random.Range(b.min.z, b.max.z));
        }

        // =========================================================
        // 1~8번 패턴부
        // =========================================================
        private IEnumerator Pattern1_Horizontal(int lines)
        {
            for (int r = 0; r < lines; r++)
            {
                for (int c = 0; c < 8; c++) SpawnRandomMonster(GetPosByRatio(spawnArea, (float)c / 7), targetArea.transform.position);
                yield return new WaitForSeconds(0.8f);
            }
        }
        private IEnumerator Pattern2_Vertical(int columns)
        {
            float[] ratios = columns == 5 ? new float[] { 0.1f, 0.3f, 0.5f, 0.7f, 0.9f } : new float[] { 0.5f };
            for (int m = 0; m < 20; m++)
            {
                foreach (var r in ratios) SpawnRandomMonster(GetPosByRatio(spawnArea, r), targetArea.transform.position);
                yield return new WaitForSeconds(0.3f);
            }
        }
        private IEnumerator Pattern3_V(int lines)
        {
            for (int v = 0; v < lines; v++)
            {
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.2f), targetArea.transform.position);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.8f), targetArea.transform.position); yield return new WaitForSeconds(0.2f);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.35f), targetArea.transform.position);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.65f), targetArea.transform.position); yield return new WaitForSeconds(0.2f);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.5f), targetArea.transform.position);
                yield return new WaitForSeconds(1.0f);
            }
        }
        private IEnumerator Pattern4_InvertedV(int lines)
        {
            for (int v = 0; v < lines; v++)
            {
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.5f), targetArea.transform.position); yield return new WaitForSeconds(0.2f);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.35f), targetArea.transform.position);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.65f), targetArea.transform.position); yield return new WaitForSeconds(0.2f);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.2f), targetArea.transform.position);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.8f), targetArea.transform.position);
                yield return new WaitForSeconds(1.0f);
            }
        }
        private IEnumerator Pattern5_X(int lines)
        {
            for (int x = 0; x < lines; x++)
            {
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.2f), targetArea.transform.position);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.8f), targetArea.transform.position); yield return new WaitForSeconds(0.3f);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.5f), targetArea.transform.position); yield return new WaitForSeconds(0.3f);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.2f), targetArea.transform.position);
                SpawnRandomMonster(GetPosByRatio(spawnArea, 0.8f), targetArea.transform.position);
                yield return new WaitForSeconds(1.0f);
            }
        }
        private IEnumerator Pattern6_WideZigzag()
        {
            for (int i = 0; i < 40; i++)
            {
                float xRatio = 0.5f + Mathf.Sin(i * 0.5f) * 0.4f;
                SpawnRandomMonster(GetPosByRatio(spawnArea, xRatio), targetArea.transform.position);
                yield return new WaitForSeconds(0.15f);
            }
        }
        private IEnumerator Pattern7_Random()
        {
            for (int i = 0; i < 30; i++)
            {
                SpawnRandomMonster(GetPosByRatio(spawnArea, Random.value), targetArea.transform.position);
                yield return new WaitForSeconds(0.15f);
            }
        }
        private IEnumerator Pattern8_AirDrop()
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 groundPos = GetRandomPosInArea(pathArea);
                SpawnRandomMonster(groundPos, targetArea.transform.position, true);
                yield return new WaitForSeconds(0.3f);
            }
        }

        private void OnDrawGizmos()
        {
            if (spawnArea != null && spawnArea.GetComponent<Renderer>()) { Gizmos.color = new Color(1, 0, 0, 0.3f); Gizmos.DrawCube(spawnArea.transform.position, spawnArea.GetComponent<Renderer>().bounds.size); }
            if (pathArea != null && pathArea.GetComponent<Renderer>()) { Gizmos.color = new Color(1, 1, 0, 0.2f); Gizmos.DrawCube(pathArea.transform.position, pathArea.GetComponent<Renderer>().bounds.size); }
            if (targetArea != null && targetArea.GetComponent<Renderer>()) { Gizmos.color = new Color(0, 0, 1, 0.3f); Gizmos.DrawCube(targetArea.transform.position, targetArea.GetComponent<Renderer>().bounds.size); }
        }
    }

    // =========================================================
    // 낙하 물리 스크립트
    // =========================================================
    public class SimpleMove : MonoBehaviour
    {
        public Vector3 direction;
        public float speed;
        public bool isAirDrop;
        private float groundLevel;

        void Start()
        {
            Destroy(gameObject, 15f);
        }

        public void Initialize(Vector3 dir, float spd, bool airDrop, float groundY)
        {
            direction = dir;
            speed = spd;
            isAirDrop = airDrop;
            groundLevel = groundY;
        }

        void Update()
        {
            if (isAirDrop)
            {
                if (transform.position.y > groundLevel)
                {
                    transform.Translate(Vector3.down * speed * 2.5f * Time.deltaTime, Space.World);
                }
                else
                {
                    Vector3 pos = transform.position;
                    pos.y = groundLevel;
                    transform.position = pos;
                    isAirDrop = false;
                }
            }
            else
            {
                transform.Translate(direction * speed * Time.deltaTime, Space.World);
            }
        }
    }
}