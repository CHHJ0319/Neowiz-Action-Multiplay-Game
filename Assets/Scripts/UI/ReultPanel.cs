using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ReultPanel : MonoBehaviour
    {
        public Button closeButton;
        public Button gameQuitButton;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => CloseResultPanel());
            }
        
            if (gameQuitButton != null)
            {
                gameQuitButton.onClick.AddListener(() => Events.GameEvents.QuitGame());
            }
        }

        void CloseResultPanel()
        {
            gameObject.SetActive(false);
        }
    }
}
