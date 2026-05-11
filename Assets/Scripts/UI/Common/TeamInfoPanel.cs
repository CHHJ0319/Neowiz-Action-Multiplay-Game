using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class TeamInfoPanel : MonoBehaviour
    {
        public TextMeshProUGUI teamName;
        public RectTransform memberList;
        public TextMeshProUGUI roundText;
        public TextMeshProUGUI scoreText;
        public Button closeButton;

        private void Awake()
        {
            if (closeButton != null)
            {
                closeButton.onClick.AddListener(() => SetVisible(false));
            }
        }

        public void SetData(Data.TeamData teamData)
        {
            teamName.text = teamData.teamName;

            for (int i = 0; i < teamData.memberNames.Count; i++)
            {
                if (i < memberList.childCount)
                {
                    var memberText = memberList.GetChild(i).GetComponent<TextMeshProUGUI>();
                    if (memberText != null)
                    {
                        memberText.text = teamData.memberNames[i];
                    }
                }
            }

            roundText.text = "" + teamData.finalRound;
            scoreText.text = "" + teamData.totalScore;
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
}