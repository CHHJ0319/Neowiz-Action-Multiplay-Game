using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.PrologueScene
{
    public class ProloguePanel : MonoBehaviour
    {
        public RectTransform prologueMask;
        public TextMeshProUGUI prologueText;
        public float scrollSpeed = 30f;

        private Vector2 startPosition;
        private float resetThreshold;

        private void Start()
        {
            SetPrologue();
            SetPrologueScroll();
        }

        private void Update()
        {
            if (prologueText != null)
            {
                prologueText.rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

                if (prologueText.rectTransform.anchoredPosition.y > resetThreshold)
                {
                    prologueText.rectTransform.anchoredPosition = startPosition;
                }
            }
        }

        private void SetPrologue()
        {
            if (prologueText != null)
            {
                string script = LocalizationSettings.StringDatabase.GetLocalizedString(
                "CommonTextTable", "PrologueScript");
                prologueText.text = script;
            }

        }

        private void SetPrologueScroll()
        {
            if (prologueText == null || prologueMask == null)
            {
                return;
            }

            float maskHeight = prologueMask.rect.height;
            float textHeight = prologueText.rectTransform.rect.height;

            startPosition = prologueText.rectTransform.anchoredPosition;

            resetThreshold = maskHeight + (textHeight / 2f);
        }
    }


}