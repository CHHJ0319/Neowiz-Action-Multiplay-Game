using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class CommonPopup : MonoBehaviour
    {
        public TextMeshProUGUI message;
        public Button confirmButton;
        public Button cancelButton;

        void Awake()
        {
            SetupButtons();
        }

        public virtual void Initialize()
        {

        }

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

        public void SetMessage(string message)
        {
            this.message.text = message;  
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