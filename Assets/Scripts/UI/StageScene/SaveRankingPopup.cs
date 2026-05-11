using System.Collections.Generic;
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
            Data.TeamData myTeam = new Data.TeamData
            {
                teamName = "슈퍼스타팀",
                memberNames = new List<string> { "철수", "영희", "민수", "바둑이" },
                finalRound = 8,
                totalScore = 1250
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