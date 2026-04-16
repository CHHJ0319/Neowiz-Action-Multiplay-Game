using UnityEngine;

namespace Actor
{
    public class PlayerField : MonoBehaviour
    {
        public static PlayerField Instance;

        public MeshFilter itemSpawnArea;
        public Transform core;

        public float hp;

        private float maxHP = 100;

        [Header("Audio Settings")]
        public AudioClip[] hitSounds;
        private AudioSource audioSource;

        private void Awake()
        {
            Instance = this;

            ResetPlayerFiledHP();

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

        public float GetPlayerFiledHPRate()
        {
            return hp / maxHP;
        }

        public void ResetPlayerFiledHP()
        {
            hp = maxHP;
            UIManager.Instance.UpdateBarricadeHPBarClientRpc(hp / maxHP);
        }

        private void TakeDamage(float damage)
        {
            hp -= damage;

            PlayHitSounds();

            UIManager.Instance.UpdateBarricadeHPBarClientRpc(hp/maxHP);

            if(hp <= 0)
            {
                ActorManager.Instance.ClearEemiesServerRpc();
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
