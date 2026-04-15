using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class ResultPanel : MonoBehaviour
    {
        [Header("TitleImage")]
        public GameObject victoryImage;
        public GameObject defeatImage;

        [Header("")]
        public RectTransform starRow;
        public TextMeshProUGUI mvpNameText;

        [Header("Buttons")]
        public RectTransform menuRow;
        public Button nextWaveButton;

        void Awake()
        {
            SetTitleImage(true);

            if (nextWaveButton != null)
            {
                nextWaveButton.onClick.AddListener(() => OnNextWaveButtonClicked());
            }
        }

        public void Initialize(bool isHost)
        {
            if(isHost)
            {
                menuRow.gameObject.SetActive(true);
            }
            else
            {
                menuRow.gameObject.SetActive(false);
            }
        }

        public void SetTitleImage(bool isVictory)
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

        public void SetStars(int starCount)
        {
            int totalChildren = starRow.childCount;

            for (int i = 0; i < totalChildren; i++)
            {
                starRow.GetChild(i).gameObject.SetActive(i < starCount);
            }
        }

        public void SetMVP(string playerrName)
        {
            if (mvpNameText != null)
            {
                mvpNameText.text = $"MVP: {playerrName}";
            }
        }

        private void OnNextWaveButtonClicked()
        {
            UIManager.Instance.CloseResultPanelClientRpc();
            StageManager.Instance.UpdateWaveIndexServerRpc();
        }
    }

}