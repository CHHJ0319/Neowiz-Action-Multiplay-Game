
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class CreateSessionPanel : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private TMP_InputField teamNameInputField;
        [SerializeField] private TMP_InputField passwordInputField;

        [Header("Button Group")]
        [SerializeField] private Button creasteSessionButton;
        [SerializeField] private Button closeButton;

        void Awake()
        {
            creasteSessionButton.onClick.AddListener(() => OnCreateSessionButtonClicked());
            closeButton.onClick.AddListener(() => OnCloseButtonClicked());
        }

        private void OnCreateSessionButtonClicked()
        {
            string playerName = playerNameInputField.text;
            string teamName = teamNameInputField.text;
            string password = passwordInputField.text;

            Events.GameEvents.StartHost(playerName, teamName, password);

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