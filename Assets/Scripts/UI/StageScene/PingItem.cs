using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class PingItem : MonoBehaviour
    {
        public TextMeshProUGUI requestMessageText;
        public Image typeIcon;
        public TextMeshProUGUI playerNameText;

        public void SetRequestMessageText(int index)
        {
            StopAllCoroutines();

            requestMessageText.text = Data.RequestData.MessageList[index];

            requestMessageText.gameObject.SetActive(true);
            StartCoroutine(HideMessageAfterDelay(1.0f));
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

        private void SetPlayerNameText(string name)
        {
            playerName.text = name;
        }

        private IEnumerator HideMessageAfterDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            requestMessageText.gameObject.SetActive(false);
        }
    }
}