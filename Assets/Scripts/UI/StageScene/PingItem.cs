using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class PingItem : MonoBehaviour
    {
        public Image speechBubble;
        public TextMeshProUGUI requestMessageText;
        public Image typeIcon;
        public TextMeshProUGUI playerNameText;

        private void Start()
        {
            SetPlayerNameText();
        }

        public void UpdateRequestMessageText(string message)
        {
            StopAllCoroutines();

            requestMessageText.text = message;

            speechBubble.gameObject.SetActive(true);
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(HideMessageAfterDelay(1.0f));
            }
        }

        public void Initialize(Data.PlayerRole role, Data.ElementType type)
        {
            if(role == Data.PlayerRole.Shooter)
            {
                this.gameObject.SetActive(true);
                SetTypeIcon(type);
            }
            else if (role == Data.PlayerRole.Supporter)
            {
                this.gameObject.SetActive(false);
            }
        }

        private void SetTypeIcon(Data.ElementType type)
        {
            switch(type)
            {
                case Data.ElementType.Red :
                    typeIcon.color = Color.red;
                    break;
                case Data.ElementType.Green:
                    typeIcon.color = Color.green;
                    break;
                case Data.ElementType.Blue:
                    typeIcon.color = Color.blue;
                    break;
            }
        }

        private void SetPlayerNameText()
        {
            playerNameText.text = "PLAYER" + (transform.GetSiblingIndex() + 1);
        }

        private IEnumerator HideMessageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            speechBubble.gameObject.SetActive(false);
        }
    }
}