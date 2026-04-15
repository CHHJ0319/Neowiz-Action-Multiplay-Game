using UnityEngine;

namespace Actor.Weapon
{
    public class Bullet : MonoBehaviour
    {
        public void Launch(Vector3 velocity)
        {
            GetComponent<Rigidbody>().linearVelocity = velocity;
            transform.forward = velocity;
        }
    }
}

