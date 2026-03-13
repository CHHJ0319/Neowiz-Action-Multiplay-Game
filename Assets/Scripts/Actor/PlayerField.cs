using UnityEngine;

namespace Actor
{
    public class PlayerField : MonoBehaviour
    {
        public float hp;

        private float maxHP = 100;

        private void Awake()
        {
            hp = maxHP;
            Events.PlayerFieldEvents.UpdateHPBar(hp/maxHP);
        }

        private void OnEnable()
        {
            Events.PlayerFieldEvents.OnEnemyCollided += TakeDamage;
        }

        private void OnDisable()
        {
            Events.PlayerFieldEvents.OnEnemyCollided -= TakeDamage;
        }

        private void TakeDamage(float damage)
        {
            hp -= damage;

            Events.PlayerFieldEvents.UpdateHPBar(hp/maxHP);

            if(hp <= 0)
            {
                RoundManager.Instance.EndRound();
            }
        }
    }
}
