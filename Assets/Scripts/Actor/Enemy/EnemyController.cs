using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyController : NetworkBehaviour
    {
        public MeshRenderer elementIcon;

        public float damage = 10;
        public float speed = 5;

        public Material[] typeMaterials;

        private int hp;

        public NetworkVariable<Data.NetworkElementType> Type = new NetworkVariable<Data.NetworkElementType>();

        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsServer)
            {
                ActorManager.Instance.AddEnemy(NetworkObjectId, this);
            }

            Type.OnValueChanged += OnTypeChanged;
        }

        public override void OnNetworkDespawn()
        {
            if(IsServer)
            {
                ActorManager.Instance.RemoveEnemy(NetworkObjectId);
            }

            Type.OnValueChanged -= OnTypeChanged;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Barricade"))
            {
                Events.ActorEvents.HandleEnemyPlayerFieldCollision(damage);
                DespawnSelfServerRPC();
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
                SetRandomElementType();
            }
            else
            {
                Type.Value = type;
            }
        }

        private void OnTypeChanged(Data.NetworkElementType previousValue, Data.NetworkElementType newValue)
        {
            SetElemetIcon();
        }

        private void SetElemetIcon()
        {
            if (elementIcon != null)
            {
                switch ((Data.ElementType)Type.Value)
                {
                    case Data.ElementType.Red:
                        elementIcon.material = typeMaterials[0];
                        break;
                    case Data.ElementType.Green:
                        elementIcon.material = typeMaterials[1];
                        break;
                    case Data.ElementType.Blue:
                        elementIcon.material = typeMaterials[2];
                        break;
                    case Data.ElementType.Yellow:
                        elementIcon.material = typeMaterials[0];
                        break;
                    case Data.ElementType.Magenta:
                        elementIcon.material = typeMaterials[1];
                        break;
                    case Data.ElementType.Cyan:
                        elementIcon.material = typeMaterials[2];
                        break;
                }
            }
        }

        public void SetHP(int lives)
        {
            this.hp = lives;
        }

        public void SetMultiType(Data.ElementType type)
        {
            if (type == Data.ElementType.Random)
            {
                SetRandomMultiType();
            }
            else
            {
                Type.Value = type;
            }
        }

        public void TakeDamage(Data.ElementType bulletType)
        {
            if(Type.Value == bulletType)
            {
                hp--;
            }

            if (hp <= 0)
            {
                DespawnSelfServerRPC();
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

        [Rpc(SendTo.Server)]
        public void DespawnSelfServerRPC(RpcParams rpcParams = default)
        {
            DespawnSelf();
        }

        private void DespawnSelf()
        {
            GetComponent<NetworkObject>().Despawn();
        }

        private void SetRandomElementType()
        {
            int randomIndex = UnityEngine.Random.Range(0, 3);

            Type.Value = (Data.ElementType) randomIndex;
        }

        private void SetRandomMultiType()
        {
            int randomIndex = UnityEngine.Random.Range(3, 6);

            Type.Value = (Data.ElementType) randomIndex;
        }
    }
}