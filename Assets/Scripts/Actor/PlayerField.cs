using UnityEngine;

namespace Actor
{
    public class PlayerField : MonoBehaviour
    {
        public Barricade[] barricades;

        public int hp;

        private void OnEnable()
        {
            
        }

        private void OnDisable()
        {
            
        }

        private void TakeDamage(int damage)
        {
            hp -= damage;
        }
    }
}
