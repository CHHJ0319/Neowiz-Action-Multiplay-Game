using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StageScene
{
    public class PlayerStatusPanel : MonoBehaviour
    {
        public TextMeshProUGUI playerNameText;
        public RectTransform playerIcons;
        public RectTransform potionIcons;
        public TextMeshProUGUI ammoText;

        private void OnEnable()
        {
            Events.PlayerEvents.OnRoleAssigned += SetPotionIcon;
        }

        private void OnDisable()
        {
            Events.PlayerEvents.OnRoleAssigned -= SetPotionIcon;
        }

        public void Initialize()
        {
            SetPlayerName(DataManager.Instance.PlayerName);
            SetPlayerIcon(DataManager.Instance.CharacterIndex);
        }
        
        private void SetPlayerName(string name)
        {
            playerNameText.text = name;
        }

        private void SetPlayerIcon(int index)
        {
            foreach (RectTransform child in playerIcons)
            {
                child.gameObject.SetActive(false);
            }

            if (index >= 0 && index < playerIcons.childCount)
            {
                playerIcons.GetChild(index).gameObject.SetActive(true);
            }
        }

        private void SetPotionIcon(Data.PlayerInfo info)
        {
            foreach (RectTransform child in potionIcons)
            {
                child.gameObject.SetActive(false);
            }

            if (info.role == Data.PlayerRole.Supporter)
            {
                potionIcons.GetChild(3).gameObject.SetActive(true);
            }
            else if ((int)info.color >= 0 && (int)info.color < potionIcons.childCount)
            {
                potionIcons.GetChild((int)info.color).gameObject.SetActive(true);
            }
        }
    }
}