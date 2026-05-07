using TMPro;
using UnityEngine;

namespace UI.LobbyScene
{
    public class TeamNamePanel : MonoBehaviour
    {
        public TextMeshProUGUI teamNameText;
        
        public void SetTeamName(string name)
        {
            if (teamNameText == null)
                return;
            
            teamNameText.text = name;
        }
    }
}