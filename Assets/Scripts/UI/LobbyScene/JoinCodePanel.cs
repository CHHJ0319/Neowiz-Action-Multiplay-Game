using TMPro;
using UI.TitleScene;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class JoinCodePanel : MonoBehaviour
    {
        public CopyJoinCodePopup copyJoinCodePopup;

        [Header("UI Elements")]
        public TextMeshProUGUI joinCode;
        public Button copyJoinCodeButton;
        public AudioClip copyJoinCodeSound;

        public float soundVolume = 1.0f;

        private void Awake()
        {
            copyJoinCodeButton.onClick.AddListener(() => OnCopyJoinCodeButtonClicked());
        }

        public void SetJoinCode()
        {
            if (joinCode != null)
            {
                gameObject.SetActive(true);
                joinCode.text = Utils.NetworkService.JoinCode;
            }
        }

        private void OnCopyJoinCodeButtonClicked()
        {
            PlayClickSound();
            if (joinCode != null)
            {
                if(copyJoinCodePopup != null)
                {
                    GUIUtility.systemCopyBuffer = joinCode.text;
                    copyJoinCodePopup.SetVisible(true);
                }
            }
        }

        private void PlayClickSound()
        {
            if (copyJoinCodeSound != null)
            {
                SoundManager.Instance.PlaySFX(copyJoinCodeSound, soundVolume);

            }
        }
    }
}