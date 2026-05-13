using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class JoinCodePanel : MonoBehaviour
    {
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
                joinCode.text = Services.NetworkService.JoinCode;
            }
        }

        private void OnCopyJoinCodeButtonClicked()
        {
            PlayClickSound();
            if (joinCode != null)
            {
                GUIUtility.systemCopyBuffer = joinCode.text;
                string message = Utils.LocaleLoader.GetPopupMessage("CopyJoinCodeMessage");
                UI.CanvasController.Instance.ShowCommonPopup(message);
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