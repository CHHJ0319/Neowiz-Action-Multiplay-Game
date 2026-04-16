using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.TitleScene
{
    public class JoinSessionPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private TMP_InputField passwordInputField;
        [SerializeField] private Button joinSessionButton;
        [SerializeField] private Button closeButton;

        void Awake()
        {
            joinSessionButton.onClick.AddListener(() => OnJoinSessionButtonClicked());
            closeButton.onClick.AddListener(() => SetVisible(false));
        }

        private void OnJoinSessionButtonClicked()
        {
            string joinCode = joinCodeInputField.text;
            //string password = passwordInputField.text;
            Events.GameEvents.StartClient(joinCode);

            SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}