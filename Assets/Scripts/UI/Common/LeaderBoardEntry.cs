using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoardEntry : MonoBehaviour
    {
        public TextMeshProUGUI rankingText;
        public TextMeshProUGUI teamName;
        public TextMeshProUGUI roundText;
        public TextMeshProUGUI scoreText;

        private Data.TeamData teamData;

        private void Awake()
        {
            Button button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => ShowTeamInfo());
            }
        }

        public void SetData(int ranking, Data.TeamData teamData)
        {
            this.teamData = teamData;

            rankingText.text = "" + ranking;
            teamName.text = teamData.teamName;
            roundText.text = "" + teamData.finalRound;
            scoreText.text = "" + teamData.totalScore;
        }

        private void ShowTeamInfo()
        {
            if (teamData == null)
                return;

            CanvasController.Instance.ShowTeamInfoPanel(teamData);
        }
    }
}