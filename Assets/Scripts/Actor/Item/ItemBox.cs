using Unity.Netcode;
using UnityEngine;

namespace Actor.Item
{
    public abstract class ItemBox : NetworkBehaviour
    {
        [Header("Ground Check Settings")]
        [SerializeField] private LayerMask groundLayer;
        [SerializeField] private float extraDistance = 0.5f;

        private Rigidbody rb;

        public bool IsGrounded = false;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Floor"))
            {
                IsGrounded = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Floor"))
            {
                IsGrounded = false;
            }
        }

        public override void OnNetworkSpawn()
        {
            
        }

        public override void OnNetworkDespawn()
        {

        }

        private void FixedUpdate()
        {
            if (!IsServer) return;

            CheckGroundStatus();
        }

        private void CheckGroundStatus()
        {
            if (IsGrounded)
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;

                transform.rotation = Quaternion.identity;
                rb.constraints = RigidbodyConstraints.FreezeAll;
            }
        }
    }
}
