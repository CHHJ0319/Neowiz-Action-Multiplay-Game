using UnityEngine;

namespace UI.PrologueScene
{
    public class ProloguePanel : MonoBehaviour
    {
        public RectTransform prologueMask;
        public RectTransform prologueText;
        public float scrollSpeed = 30f;

        private Vector2 startPosition;
        private float resetThreshold;

        private void Start()
        {
            if(prologueText != null && prologueMask != null)
        {
                float maskHeight = prologueMask.rect.height;
                float textHeight = prologueText.rect.height;

                startPosition = new Vector2(prologueText.anchoredPosition.x, prologueText.anchoredPosition.y);

                resetThreshold = maskHeight + textHeight / 2f;
            }
        }

        private void Update()
        {
            if (prologueText != null)
            {
                prologueText.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

                if (prologueText.anchoredPosition.y > resetThreshold)
                {
                    prologueText.anchoredPosition = startPosition;
                }
            }
        }
    }
}