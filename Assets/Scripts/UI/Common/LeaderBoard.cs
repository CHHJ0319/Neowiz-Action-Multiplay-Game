using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoard : MonoBehaviour
    {
        public Button closeButton;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => SetVisible(false));
            }
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}