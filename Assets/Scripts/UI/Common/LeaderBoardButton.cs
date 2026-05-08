using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoardButton : MonoBehaviour
    {
        private void Awake()
        {
            Button btn = GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.AddListener(() => OpenLeaderBoard());
            }
        }
        private void OpenLeaderBoard()
        {
            
        }
    }
}