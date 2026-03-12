using UnityEngine;

namespace Actor 
{
    public class EnemyController : MonoBehaviour
    {
        private float HP;

        public void TakeDamage()
        {
            DestroySelf();
        }

        public void DestroySelf()
        {
            //Destroy(gameObject);
        }
    }
}
