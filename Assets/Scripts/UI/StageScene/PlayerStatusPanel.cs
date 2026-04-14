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
            Events.PlayerEvents.OnAmmoChanged += UpdateAmmoText;
        }

        private void OnDisable()
        {
            Events.PlayerEvents.OnRoleAssigned -= SetPotionIcon;
            Events.PlayerEvents.OnAmmoChanged -= UpdateAmmoText;
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

        private void SetPotionIcon(Data.PlayerRole role, Data.ElementType type)
        {
            foreach (RectTransform child in potionIcons)
            {
                child.gameObject.SetActive(false);
            }

            if (role == Data.PlayerRole.Supporter) return;

            if ((int)type >= 0 
                && (int)type < potionIcons.childCount)
            {
                potionIcons.GetChild((int)type).gameObject.SetActive(true);
            }
        }

        private void UpdateAmmoText(int ammo)
        {
            if (ammoText == null) return;

            if(ammo == 0)
            {
                ammoText.text = "";   
            }
            else
            {
                ammoText.text = "x" + ammo;
            }
        }
    }
}