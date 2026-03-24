using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI 
{
    public class NetworPanel : MonoBehaviour
    {
        [SerializeField] private TMP_InputField joinCodeInputField;
        [SerializeField] private Button accessButton;

        void Awake()
        {
            accessButton.onClick.AddListener(Access);
        }

        private void Access()
        {
            string joinCode = joinCodeInputField.text;

            if (!string.IsNullOrEmpty(joinCode))
            {
                Events.GameEvents.StartClient(joinCode);
            }
            else
            {
                Events.GameEvents.StartHost();
            }
        }
    }
}
