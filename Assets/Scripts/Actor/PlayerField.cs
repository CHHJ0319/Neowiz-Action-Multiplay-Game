using UnityEngine;

namespace Actor
{
    public class PlayerField : MonoBehaviour
    {
        public static PlayerField Instance;

        public MeshFilter plane;
        public Transform core;

        public float hp;

        private float maxHP = 100;

        private void Awake()
        {
            Instance = this;

            hp = maxHP;
            Events.ActorEvents.UpdatePlayerFieldHPBar(hp/maxHP);
        }

        private void OnEnable()
        {
            Events.ActorEvents.OnEnemyEnteredPlayerField += TakeDamage;
        }

        private void OnDisable()
        {
            Events.ActorEvents.OnEnemyEnteredPlayerField -= TakeDamage;
        }

        private void TakeDamage(float damage)
        {
            hp -= damage;

            Events.ActorEvents.UpdatePlayerFieldHPBar(hp/maxHP);

            if(hp <= 0)
            {
                //Events.RoundEvents.EndRound();
            }
        }
    }
}
