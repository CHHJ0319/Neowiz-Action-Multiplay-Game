using Unity.Netcode;
using Unity.Netcode.Components;
using UnityEngine;

namespace Actor.Item
{
    public abstract class ItemBox : NetworkBehaviour
    {
        private Rigidbody rb;

        public bool IsGrounded = false;

        private int floorLayer;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            floorLayer = LayerMask.NameToLayer("Floor");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.layer == floorLayer)
            {
                IsGrounded = true;
            }

            if (IsGrounded)
            {
                Rigidbody rb = GetComponent<Rigidbody>();

                if (rb != null)
                {
                    rb.linearVelocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;

                    transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);

                    rb.constraints = RigidbodyConstraints.FreezePositionX |
                                     RigidbodyConstraints.FreezePositionZ |
                                     RigidbodyConstraints.FreezeRotation;
                }
            }
            else
            {
                if (collision.gameObject.CompareTag("Player"))
                {
                    if (transform.parent == null)
                    {
                        Actor.Player.PlayerController player = collision.gameObject.GetComponent<Actor.Player.PlayerController>();
                        Use(player);
                    }
                }
            }
        }

        private void OnCollisionExit(Collision collision)
        {
            if (collision.gameObject.layer == floorLayer)
            {
                IsGrounded = false;
            }
        }

        public abstract void Use(Actor.Player.PlayerController player);
    }
}
