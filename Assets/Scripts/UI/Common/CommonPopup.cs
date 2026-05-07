using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.LobbyScene
{
    public abstract class CommonPopup : MonoBehaviour
    {
        public TextMeshProUGUI prompt;
        public Button confirmButton;
        public Button cancelButton;

        protected virtual void SetupButtons()
        {
            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(() =>
                {
                    OnPopupButtonClicked();
                });
            }

            if (cancelButton != null)
            {
                cancelButton.onClick.AddListener(() =>
                {
                    OnPopupButtonClicked();
                });
            }
        }

        protected void SetPrompt(string message)
        {
            prompt.text = message;  
        }

        private void OnPopupButtonClicked()
        {
            SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}