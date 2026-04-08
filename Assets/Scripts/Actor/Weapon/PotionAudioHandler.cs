using UnityEngine;

namespace Actor.Weapon
{
    public class PotionAudioHandler : MonoBehaviour
    {
        private AudioSource _audioSource;

        [Header("Audio Clips")]
        public AudioClip breakSound; 

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public void PlayBreakSound()
        {
            if (breakSound != null && _audioSource != null)
            {
                _audioSource.PlayOneShot(breakSound);
            }
        }
    }
}