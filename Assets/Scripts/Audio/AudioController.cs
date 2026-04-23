using UnityEngine;

namespace Audio
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        public AudioSource bgmSource;

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
            if (bgmTracks == null && bgmTracks.Length <= 0 && bgmSource == null) return;

            bgmSource.clip = bgmTracks[0];
            bgmSource.loop = true;
            bgmSource.Play();
        }
    }
}
