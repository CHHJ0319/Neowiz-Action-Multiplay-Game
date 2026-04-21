using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class CreateSessionPanel : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private RectTransform playerNameInputFieldErrorMessage;
        [SerializeField] private TMP_InputField teamNameInputField;
        [SerializeField] private RectTransform teamNameInputFieldErrorMessage;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private RectTransform passwordInputFieldErrorMessage;

        [Header("Button Group")]
        [SerializeField] private Button creasteSessionButton;
        [SerializeField] private Button closeButton;

        void Awake()
        {
            creasteSessionButton.onClick.AddListener(() => OnCreateSessionButtonClicked());
            closeButton.onClick.AddListener(() => OnCloseButtonClicked());

            playerNameInputField.characterLimit = 5;
            playerNameInputField.onEndEdit.AddListener((value) => CheckIfEmpty(value, playerNameInputFieldErrorMessage));
            teamNameInputField.characterLimit = 10;
            teamNameInputField.onEndEdit.AddListener((value) => CheckIfEmpty(value, teamNameInputFieldErrorMessage));
            passwordInputField.characterLimit = 4;
            //passwordInputField.onEndEdit.AddListener((value) => CheckIfEmpty(value, passwordInputFieldErrorMessage));
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

        private void CheckIfEmpty(string input, RectTransform errorMessage)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                errorMessage.gameObject.SetActive(true);
            }
            else
            {
                errorMessage.gameObject.SetActive(false);
            }
        }
    }
}