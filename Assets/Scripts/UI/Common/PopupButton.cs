using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class PopupButton : MonoBehaviour
    {
        public AudioClip clickSound;
        public float soundVolume = 1.0f;

        private void Awake()
        {
            Button btn = GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.AddListener(() => PlayClickSound());
            }
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