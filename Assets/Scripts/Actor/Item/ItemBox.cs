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
        private Collider _collider;

        public NetworkVariable<bool> IsGrounded = new NetworkVariable<bool>(
            false,
            NetworkVariableReadPermission.Everyone,
            NetworkVariableWritePermission.Server
        );

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            _collider = GetComponent<Collider>();
        }

        public override void OnNetworkSpawn()
        {
            IsGrounded.OnValueChanged += OnGroundedStateChanged;
        }

        public override void OnNetworkDespawn()
        {
            IsGrounded.OnValueChanged -= OnGroundedStateChanged;
        }

        private void FixedUpdate()
        {
            if (!IsServer) return;

            CheckGroundStatus();
        }

        private void CheckGroundStatus()
        {
            float boxHalfHeight = GetComponent<Collider>().bounds.extents.y; ;
            float rayLength = boxHalfHeight + extraDistance;

            RaycastHit hitInfo;
            bool hit = Physics.Raycast(transform.position, Vector3.down, out hitInfo, rayLength, groundLayer);
            Debug.DrawRay(transform.position, Vector3.down * rayLength, hit ? Color.green : Color.red);

            if (IsGrounded.Value != hit)
            {
                if (IsServer)
                {
                    IsGrounded.Value = hit;

                    if (hit)
                    {
                        rb.linearVelocity = Vector3.zero;
                        rb.angularVelocity = Vector3.zero;
                        transform.position = new Vector3(transform.position.x, 0.5f, transform.position.z);
                        GetComponent<Collider>().enabled = true;
                        rb.constraints = RigidbodyConstraints.FreezeAll;
                    }
                    else
                    {
                        GetComponent<Collider>().enabled = false;
                        rb.constraints = RigidbodyConstraints.FreezeRotation;
                    }
                }
            }
        }

        private void OnGroundedStateChanged(bool previousValue, bool newValue)
        {
            if (newValue) Debug.Log("Bullet Box has landed on the ground.");
        }
    }
}
