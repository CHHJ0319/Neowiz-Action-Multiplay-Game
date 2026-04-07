using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyController : NetworkBehaviour
    {
        public MeshRenderer ringTarget;

        public float damage = 10;
        public float speed = 5;

        [Header("Type Materials")]
        public Material typeRed;
        public Material typeGreen;
        public Material typeBlue;

        [Header("HP")]
        public int hpRed = 1;
        public int hpGreen = 1;
        public int hpBlue = 1;
        public TextMeshPro hpRedText;
        public TextMeshPro hpGreenText;
        public TextMeshPro hpBlueText;
        private int totalHP;

        public NetworkList<Data.NetworkElementType> Types = new NetworkList<Data.NetworkElementType>();

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();

            CalculateTotalHP();
            UpdateHPTexts();
        }

        public override void OnNetworkSpawn()
        {
            Types.OnListChanged += OnTypesChanged;

            if (Types.Count > 0)
            {
                UpdateVisuals(Types[Types.Count - 1]);
            }
        }

        public override void OnNetworkDespawn()
        {
            Types.OnListChanged -= OnTypesChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
            }
            else if (other.CompareTag("Barricade"))
            {
                Events.ActorEvents.HandleEnemyPlayerFieldCollision(damage);
                Destroy(gameObject);
            }
        }

        [Rpc(SendTo.Server)]
        public void SetTypeServerRPC(Data.ElementType type, RpcParams rpcParams = default)
        {
            SetType(type);
        }

        public void SetType(Data.ElementType type)
        {
            if(type == Data.ElementType.Random)
            {
                Types.Add(GetRandomElementType());
            }
            else
            {
                Types.Add(type);
            }

            //SetRingTarget(Types[^1]);
            //SetHPtexts(Types[^1]);
        }

        private void OnTypesChanged(NetworkListEvent<Data.NetworkElementType> changeEvent)
        {
            if (changeEvent.Type == NetworkListEvent<Data.NetworkElementType>.EventType.Add)
            {
                UpdateVisuals(changeEvent.Value);
            }
        }

        private void UpdateVisuals(Data.ElementType type)
        {
            SetRingTarget(type);
            SetHPtexts(type);
        }

        private void SetHealthByType(Data.ElementType type, int amount)
        {
            switch (type)
            {
                case Data.ElementType.Red:
                    hpRed = amount;
                    break;

                case Data.ElementType.Green:
                    hpGreen = amount;
                    break;

                case Data.ElementType.Blue:
                    hpBlue = amount;
                    break;
            }
        }

        public void SetMultipleLives()
        {
            foreach (Data.ElementType myType in Types)
            {
                SetHealthByType(myType, 3);
            }

            CalculateTotalHP();
            UpdateHPTexts();
        }

        public void SetMultiType(Data.ElementType[] types)
        {
            if(types.Length < 2)
            {
                SetRandomMultiType();
            }
            else
            {
                foreach(var type in types)
                {
                    if (!Types.Contains(type))
                    {
                        Types.Add(type);
                        SetRingTarget(type);
                        SetHPtexts(type);
                    }
                }
            }

            CalculateTotalHP();
            UpdateHPTexts();
        }

        public void TakeDamage(Data.ElementType type)
        {
            if (Types.Contains(type))
            {
                switch (type)
                {
                    case Data.ElementType.Red:
                        if (hpRed > 0) hpRed--;
                        break;
                    case Data.ElementType.Green:
                        if (hpGreen > 0) hpGreen--;
                        break;
                    case Data.ElementType.Blue:
                        if (hpBlue > 0) hpBlue--;
                        break;
                }

                CalculateTotalHP();
                UpdateHPTexts();
            }

            if (totalHP <= 0)
            {
                DestroySelf();
            }
        }

        [Rpc(SendTo.Everyone)]
        public void LaunchClientRpc(Vector3 direction)
        {
            Launch(direction);
        }

        public void Launch(Vector3 direction)
        {
            if (rb != null)
            {
                rb.linearVelocity = direction * speed;
            }
        }

        private void DestroySelf()
        {
            Destroy(gameObject);
        }

        #region HPText
        private void SetHPtexts(Data.ElementType type)
        {
            if (type == Data.ElementType.Red)
            {
                hpRedText.gameObject.SetActive(true);
            }
            if (type == Data.ElementType.Green)
            {
                hpGreenText.gameObject.SetActive(true);
            }
            if (type == Data.ElementType.Blue)
            {
                hpBlueText.gameObject.SetActive(true);
            }
        }

        private void UpdateHPTexts()
        {
            hpRedText.text = "" + hpRed;
            hpGreenText.text = "" + hpGreen;
            hpBlueText.text = "" + hpBlue;
        }
        #endregion

        private void SetRingTarget(Data.ElementType type)
        {
            if (ringTarget != null)
            {
                switch (type)
                {
                    case Data.ElementType.Red:
                        ringTarget.material = typeRed;
                        break;
                    case Data.ElementType.Green:
                        ringTarget.material = typeGreen;
                        break;
                    case Data.ElementType.Blue:
                        ringTarget.material = typeBlue;
                        break;
                }
            }
        }

        private void CalculateTotalHP()
        {
            totalHP = 0;

            if (Types.Contains(Data.ElementType.Red))
            {
                totalHP += hpRed;
            }

            if (Types.Contains(Data.ElementType.Green))
            {
                totalHP += hpGreen;
            }

            if (Types.Contains(Data.ElementType.Blue))
            {
                totalHP += hpBlue;
            }
        }

        private Data.ElementType GetRandomElementType()
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);

            if (randomIndex == 0) return Data.ElementType.Red;
            else if (randomIndex == 1) return Data.ElementType.Green;
            else return Data.ElementType.Blue;
        }

        private void SetRandomMultiType()
        {
            int targetTypeCount = UnityEngine.Random.Range(2, 4);

            while (Types.Count < targetTypeCount)
            {
                Data.ElementType newType = GetRandomElementType();

                if (!Types.Contains(newType))
                {
                    Types.Add(newType);
                    SetRingTarget(newType);
                    SetHPtexts(newType);
                }
            }
        }
    }
}