using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public class JoinCodePanel : MonoBehaviour
    {
        public TextMeshProUGUI joinCode;
        public Button copyJoinCodeButton;
        public AudioClip clickSound;

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
                GUIUtility.systemCopyBuffer = joinCode.text;

            }
        }

        private void PlayClickSound()
        {
            if (clickSound != null)
            {
                Vector3 cameraPos = Camera.main.transform.position;
                AudioSource.PlayClipAtPoint(clickSound, cameraPos);
            }
        }
    }
}