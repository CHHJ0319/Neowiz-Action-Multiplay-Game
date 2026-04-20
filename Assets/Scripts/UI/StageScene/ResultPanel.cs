using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class ResultPanel : MonoBehaviour
    {
        [Header("StarRow")]
        public RectTransform victoryStarDeco;
        public RectTransform lostStarDeco;
        public RectTransform starRow;
        public Sprite activeStarSprite;
        public Sprite inactiveStarSprite;

        [Header("TitleRow")]
        public GameObject victoryImage;
        public GameObject defeatImage;

        [Header("MVPRow")]
        public RectTransform mvpImage;
        public RectTransform failImage;
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

        public void ShowResult(int startCount, string mvp)
        {
            if(startCount > 0)
            {
                SetTitleImage(true);
                SetStarDeco(true);
                nextWaveButton.gameObject.SetActive(true);
                SetMVPImage(true);
                SetMVPName(mvp);
            }
            else
            {
                SetTitleImage(false);
                SetStarDeco(false);
                nextWaveButton.gameObject.SetActive(false);
                SetMVPImage(false);
                SetMVPName("");
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

        private void SetStarDeco(bool isVictory)
        {
            victoryStarDeco.gameObject.SetActive(false);
            lostStarDeco.gameObject.SetActive(false);

            if (isVictory)
            {
                victoryStarDeco.gameObject.SetActive(true);
            }

            else
            {
                lostStarDeco.gameObject.SetActive(true);
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

        private void SetMVPImage(bool isVictory)
        {
            mvpImage.gameObject.SetActive(false);
            failImage.gameObject.SetActive(false);

            if (isVictory)
            {
                mvpImage.gameObject.SetActive(true);
            }

            else
            {
                failImage.gameObject.SetActive(true);
            }
        }

        private void SetMVPName(string name)
        {
            if (mvpNameText != null)
            {
                mvpNameText.text = $"MVP: {name}";
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