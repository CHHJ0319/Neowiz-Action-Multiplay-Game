using Data;
using Services;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V20;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class SaveRankingPopup : MonoBehaviour
    {
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

        private void OnSaveButtonClicked()
        {
            TeamData myTeam = new TeamData
            {
                teamName = "슈퍼스타팀",
                memberNames = new List<string> { "철수", "영희", "민수" },
                finalRound = 8,
                totalScore = 1250
            };
            _ = UGSService.SubmitTeamScoreAsync(myTeam);

            // SetVisible(false);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}