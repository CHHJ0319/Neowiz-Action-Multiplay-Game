using UnityEngine;
using Actor.Player;

namespace Actor.Player
{
    public class PlayerAudioHandler : MonoBehaviour
    {
        private AudioSource _audioSource;

        [Header("Audio Clips")]
        public AudioClip dashSound;
        public AudioClip throwSound;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        private void PlayDashSound()
        {
            PlaySFX(dashSound);
        }

        private void PlayAttackSound()
        {
            PlaySFX(throwSound);
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