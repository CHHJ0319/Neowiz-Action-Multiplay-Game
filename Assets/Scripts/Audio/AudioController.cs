using UnityEngine;
using UnityEngine.UI;

namespace Audio
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager instance;

        public AudioSource bgmSource;
        public AudioSource buttonSource;

        [Header("BGM Settings")]
        public AudioClip[] bgmTracks;

        [Header("Connection Settings")]
        public RectTransform buttons;

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
            }
        }

        private void Start()
        {
            PlayBGM();
        }

        private void Initialized()
        {
            AssignButtonSounds();
        }

        public void AssignButtonSounds()
        {
            if (buttons == null) return;

            foreach (RectTransform btn in buttons)
            {
                btn.GetComponent<Button>().onClick.AddListener(PlayButtonClickSound);
            }
        }

        private void PlayBGM()
        {
            if (bgmTracks == null && bgmTracks.Length <= 0 && bgmSource == null) return;

            bgmSource.clip = bgmTracks[0];
            bgmSource.loop = true;
            bgmSource.Play();
        }

        private void PlayButtonClickSound()
        {
            if (buttonSource != null)
            {
                buttonSource.Play();
            }
        }
    }
}
