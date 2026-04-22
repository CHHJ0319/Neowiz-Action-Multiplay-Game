using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI 
{
    public class NetworkPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private Button accessButton;
        [SerializeField] private TextMeshProUGUI joinCode;

        void Awake()
        {
            accessButton.onClick.AddListener(() => Access());
        }

        private void OnEnable()
        {
            Events.UIEvents.OnJoinCodeGenerated += SetJoinCode;
        }

        private void OnDisable()
        {
            Events.UIEvents.OnJoinCodeGenerated -= SetJoinCode;
        }

        private void Access()
        {
            string playerName = "TestPlayer";
            string teamName = "TestTeam";
            string password = "0000";

            string joinCode = joinCodeInputField.text;

            if (!string.IsNullOrEmpty(joinCode))
            {
                Events.GameEvents.StartClient(joinCode, playerName, password);
            }
            else
            {
                Events.GameEvents.StartHost(playerName, teamName, password);
            }

            joinCodeInputField.gameObject.SetActive(false);
            accessButton.gameObject.SetActive(false);
        }

        public void SetJoinCode(string code)
        {
            joinCode.gameObject.SetActive(true);
            joinCode.text = code;
        }
    }
}
