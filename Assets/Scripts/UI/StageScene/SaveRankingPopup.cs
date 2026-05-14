using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class SaveRankingPopup : MonoBehaviour
    {
        public TextMeshProUGUI messageText;
        public TextMeshProUGUI cautionText;
        public Button saveButton;
        public Button nextWaveButton;

        private void Awake()
        {
            if (saveButton != null)
            {
                saveButton.onClick.AddListener(() => OnSaveButtonClicked());
            }

            if (nextWaveButton != null)
            {
                nextWaveButton.onClick.AddListener(() => SetVisible(false));
            }
        }

        public void Initialize(int rank)
        {
            string message = Utils.LocaleLoader.GetPopupMessage("MSG_RANK_CONGRATS");
            string formattedMessage = string.Format(message, rank);
            messageText.text = formattedMessage;
            message = Utils.LocaleLoader.GetPopupMessage("MSG_SAVE_WARNING_RESTART");
            cautionText.text = message;
        }

        private void OnSaveButtonClicked()
        {
            Data.TeamData myTeam = new Data.TeamData
            {
                teamName = SessionManager.Instance.GetTeamName(),
                memberNames = SessionManager.Instance.MemberNames,
                finalRound = StageManager.Instance.GetFinalRound(),
                totalScore = SessionManager.Instance.TotalScore
            };
            DataManager.Instance.SaveRanking(myTeam);

            SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}