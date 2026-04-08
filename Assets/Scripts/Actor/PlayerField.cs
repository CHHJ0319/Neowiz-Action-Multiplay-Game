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

        [Header("Audio Settings")]
        public AudioClip[] hitSounds;
        private AudioSource audioSource;

        private void Awake()
        {
            Instance = this;

            hp = maxHP;
            Events.ActorEvents.UpdatePlayerFieldHPBar(hp/maxHP);

            audioSource = GetComponent<AudioSource>();
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

            PlayHitSounds();

            Events.ActorEvents.UpdatePlayerFieldHPBar(hp/maxHP);

            if(hp <= 0)
            {
                //Events.RoundEvents.EndRound();
            }
        }

        private void PlayHitSounds()
        {
            if (hitSounds != null && hitSounds.Length > 0)
            {
                audioSource.PlayOneShot(hitSounds[Random.Range(0, hitSounds.Length)]);
            }
        }
    }
}
