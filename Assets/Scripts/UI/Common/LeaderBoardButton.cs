using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoardButton : MonoBehaviour
    {
        public LeaderBoard leaderBoard;

        private void Awake()
        {
            Button btn = GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.AddListener(() => ShowLeaderBoard());
            }
        }
        private void ShowLeaderBoard()
        {
            if (leaderBoard == null)
                return;

            leaderBoard.SetVisible(true);
        }
    }
}