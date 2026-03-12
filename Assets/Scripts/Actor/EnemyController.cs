using UnityEngine;

namespace Actor 
{
    public class EnemyController : MonoBehaviour
    {
        private float HP;

        public EnemyType Type { get; private set; } = EnemyType.Red;

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
