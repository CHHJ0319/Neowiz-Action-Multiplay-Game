using UnityEngine;

namespace Actor.Enemy
{
    public class EnemyAudioHandler : MonoBehaviour
    {
        private AudioSource _audioSource;

        [Header("Audio Clips")]
        public AudioClip crySound;
        public AudioClip deathSound;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayCrySound()
        {
            PlaySFX(crySound);
        }

        public void PlayDeathSound()
        {
            PlaySFX(deathSound);
        }

        private void PlaySFX(AudioClip clip)
        {
            if (clip != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(clip);
            }
        }
    }
}