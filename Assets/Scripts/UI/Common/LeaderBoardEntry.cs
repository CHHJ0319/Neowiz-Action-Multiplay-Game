using UnityEngine;
using UnityEngine.UI;

namespace UI.Common
{
    public class LeaderBoardEntry : MonoBehaviour
    {
        private void Awake()
        {
            Button button = GetComponent<Button>();

            if (button != null)
            {
                button.onClick.AddListener(() => CanvasController.Instance.ShowTeamInfoPanel());
            }
        }
    }
}