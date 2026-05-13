using UnityEngine;
using UnityEngine.UI;

namespace UI.TutorialScene
{
    public class TutorialPanel : MonoBehaviour
    {
        public RectTransform Images;
        public Button previousButton;
        public Button nextButton;

        private int currentIndex = 0;
        private int totalImages;

        void Start()
        {
            totalImages = Images.childCount;

            previousButton.onClick.AddListener(() => OnPreviousButtonClicked());
            nextButton.onClick.AddListener(() => OnNextButtonClicked());

            UpdateUI();
        }

        void OnPreviousButtonClicked()
        {
            if (currentIndex > 0)
            {
                currentIndex--;
                UpdateUI();
            }
        }

        void OnNextButtonClicked()
        {
            if (currentIndex < totalImages - 1)
            {
                currentIndex++;
                UpdateUI();
            }
        }

        void UpdateUI()
        {
            for (int i = 0; i < totalImages; i++)
            {
                Images.GetChild(i).gameObject.SetActive(i == currentIndex);
            }

            previousButton.gameObject.SetActive(currentIndex > 0);

            nextButton.gameObject.SetActive(currentIndex < totalImages - 1);
        }
    }
}