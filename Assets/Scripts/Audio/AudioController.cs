using UnityEngine;

namespace Audio
{
    public class AudioController : MonoBehaviour
    {
        public static AudioController instance;

        [Header("Audio Sources")]
        public AudioSource bgmSource;
        public AudioSource sfxSource;

        [Header("BGM Lists")]
        public AudioClip[] bgmTracks;

        void Awake()
        {
            if (instance == null)
            {
                instance = this;

                Initialized();
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            PlayBGM();
        }

        private void Initialized()
        {
        }

        private void PlayBGM()
        {
            if (bgmTracks == null && bgmTracks.Length <= 0) return;

            if(bgmSource != null)
            {
                bgmSource.clip = bgmTracks[0];
                bgmSource.loop = true;
                bgmSource.Play();
            }
        }

        public void PlaySFX(AudioClip clip, float volume = 1f)
        {
            if (bgmSource != null)
            {
                sfxSource.PlayOneShot(clip, volume);
            }
        }
    }
}
