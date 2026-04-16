using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class ResultPanel : MonoBehaviour
    {
        [Header("TitleRow")]
        public GameObject victoryImage;
        public GameObject defeatImage;

        [Header("StarRow")]
        public RectTransform starRow;
        public Sprite activeStarSprite;
        public Sprite inactiveStarSprite;

        [Header("MVPRow")]
        public TextMeshProUGUI mvpNameText;

        [Header("MenuRoww")]
        public RectTransform menuRow;
        public Button restartStageButton;
        public Button nextWaveButton;

        void Awake()
        {
            SetupButtons();
        }

        private void SetupButtons()
        {
            if (restartStageButton != null)
            {
                restartStageButton.onClick.AddListener(() => OnRestartStageButtonClicked());
            }

            if (nextWaveButton != null)
            {
                nextWaveButton.onClick.AddListener(() => OnNextWaveButtonClicked());
            }
        }

        public void Initialize(bool isHost)
        {
            if (isHost)
            {
                menuRow.gameObject.SetActive(true);
            }
            else
            {
                menuRow.gameObject.SetActive(false);
            }
        }

        public void ShowResult(int startCount)
        {
            if(startCount > 0)
            {
                SetTitleImage(true);
            }
            else
            {
                SetTitleImage(false);
            }

            SetStars(startCount);
        }

        private void SetTitleImage(bool isVictory)
        {
            victoryImage.SetActive(false);
            defeatImage.SetActive(false);

            if (isVictory)
            {
                victoryImage.SetActive(true);
            }

            else
            {
                defeatImage.SetActive(true);
            }
        }

        private void SetStars(int starCount)
        {
            int totalChildren = starRow.childCount;

            for (int i = 0; i < totalChildren; i++)
            {
                if(i<starCount)
                {
                    starRow.GetChild(i).GetComponent<Image>().sprite = activeStarSprite;
                }
                else
                {
                    starRow.GetChild(i).GetComponent<Image>().sprite = inactiveStarSprite;
                }
            }
        }

        private void SetMVP(string playerName)
        {
            if (mvpNameText != null)
            {
                mvpNameText.text = $"MVP: {playerName}";
            }
        }

        private void OnRestartStageButtonClicked()
        {
            UIManager.Instance.CloseResultPanelClientRpc();
            StageManager.Instance.ResetStageServerRpc();
        }

        private void OnNextWaveButtonClicked()
        {
            UIManager.Instance.CloseResultPanelClientRpc();
            StageManager.Instance.UpdateWaveIndexServerRpc();
        }
    }

}