using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;

namespace UI.TitleScene
{
    public class SettingConfirmPanel : UI.LobbyScene.CommonPopup
    {

        protected override void Awake()
        {
            base.Awake();

            string message = LocalizationSettings.StringDatabase.GetLocalizedString("PopupMessage", "settingsConfirmMessage");
            SetPrompt(message);
        }


    }
}