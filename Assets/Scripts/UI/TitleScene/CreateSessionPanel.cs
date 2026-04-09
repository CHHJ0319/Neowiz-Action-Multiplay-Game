
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class CreateSessionPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button creasteSessionButton;
        [SerializeField] private Button closesButton;

        void Awake()
        {
            creasteSessionButton.onClick.AddListener(() => OnCreateSessionButtonClicked());
            closesButton.onClick.AddListener(() => OnCloseButtonClicked());
        }

        private void OnCreateSessionButtonClicked()
        {
            //string password = passwordInputField.text;
            Events.GameEvents.StartHost();

            SetVisible(false);
        }

        private void OnCloseButtonClicked()
        {
            SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}