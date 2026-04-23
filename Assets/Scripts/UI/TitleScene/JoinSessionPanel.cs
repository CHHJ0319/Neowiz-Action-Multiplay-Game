using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class JoinSessionPanel : MonoBehaviour
    {
        [Header("Input Fields")]
        [SerializeField] private TMP_InputField playerNameInputField;
        [SerializeField] private RectTransform playerNameInputFieldErrorMessage;
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private RectTransform joinCodeInputFieldErrorMessage;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private RectTransform passwordInputFieldErrorMessage;

        [Header("Button Group")]
        [SerializeField] private Button joinSessionButton;
        [SerializeField] private Button closeButton;

        void Awake()
        {
            joinSessionButton.onClick.AddListener(() => OnJoinSessionButtonClicked());
            closeButton.onClick.AddListener(() => SetVisible(false));

            playerNameInputField.characterLimit = 5;
            playerNameInputField.onEndEdit.AddListener((value) => CheckIfEmpty(value, playerNameInputFieldErrorMessage));
            joinCodeInputField.onEndEdit.AddListener((value) => CheckIfEmpty(value, joinCodeInputFieldErrorMessage));
            passwordInputField.characterLimit = 4;
            passwordInputField.onEndEdit.AddListener((value) => ValidatePasswordLength(value, passwordInputFieldErrorMessage));
        }

        private void Start()
        {
            playerNameInputField.onValueChanged.AddListener(delegate { OnInputChanged(); });
            joinCodeInputField.onValueChanged.AddListener(delegate { OnInputChanged(); });
            passwordInputField.onValueChanged.AddListener(delegate { OnInputChanged(); });
        }

        private void OnJoinSessionButtonClicked()
        {
            string playerName = playerNameInputField.text;
            string joinCode = joinCodeInputField.text;
            string password = passwordInputField.text;
            
            Events.GameEvents.StartClient(joinCode, playerName, password);

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

        private void ValidatePasswordLength(string input, RectTransform errorMessage)
        {
            if (input.Length == 4)
            {
                errorMessage.gameObject.SetActive(false);
            }
            else
            {
                errorMessage.gameObject.SetActive(true);
            }
        }

        private void OnInputChanged()
        {
            joinSessionButton.interactable = CanJoinSession();
        }

        private bool CanJoinSession()
        {
            bool isPlayerNameValid = !string.IsNullOrWhiteSpace(playerNameInputField.text);

            bool isJoinCodeValid = !string.IsNullOrWhiteSpace(joinCodeInputField.text);

            bool isPasswordValid = passwordInputField.text.Length == 4;

            return isPlayerNameValid && isJoinCodeValid && isPasswordValid;
        }
    }
}