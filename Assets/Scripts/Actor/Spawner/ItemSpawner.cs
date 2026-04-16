using Unity.Netcode;
using UnityEngine;

namespace Actor.Spawner
{
    public class ItemSpawner : NetworkBehaviour
    {
        public static ItemSpawner Instance;

        public GameObject[] itemBoxPrefabs;

        private float _yOffset = 1.0f;

        private void Awake()
        {
            Instance = this;
        }

        public void SpawnItem()
        {
            int randomIndex = Random.Range(0, 3);
            GameObject item = Instantiate(itemBoxPrefabs[randomIndex]);
            item.transform.localPosition = GetRandomPositionOnPlane();

            NetworkObject nv = item.GetComponent<NetworkObject>();
            nv.Spawn();
        }

        public Vector3 GetRandomPositionOnPlane()
        {
            MeshFilter targetPlane = PlayerField.Instance.itemSpawnArea;

            if (targetPlane == null) return Vector3.zero;

            Bounds bounds = targetPlane.sharedMesh.bounds;
            float randomX = Random.Range(bounds.min.x, bounds.max.x);
            float randomZ = Random.Range(bounds.min.z, bounds.max.z);

            Vector3 localPos = new Vector3(randomX, bounds.center.y + _yOffset, randomZ);
            Vector3 worldPos = targetPlane.transform.TransformPoint(localPos);

            return worldPos;
        }
    }
}
