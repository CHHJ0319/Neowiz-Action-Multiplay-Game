using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class MainMenuButton : MonoBehaviour
    {
        [Header("Audio")]
        public AudioClip clickSound;
        public float soundVolume = 1.0f;

        void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                PlayClickSound();
            });
        }

        private void PlayClickSound()
        {
            if (clickSound != null)
            {
                SoundManager.Instance.PlaySFX(clickSound, soundVolume);
            }
        }
    }
}