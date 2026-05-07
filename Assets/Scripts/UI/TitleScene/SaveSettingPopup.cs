using UnityEngine.Localization.Settings;

namespace UI.TitleScene
{
    public class SaveSettingPopup : UI.LobbyScene.CommonPopup
    {
        void Awake()
        {
            string message = LocalizationSettings.StringDatabase.GetLocalizedString(
                "PopupMessage", "saveSettingMessage");
            SetPrompt(message);

            SetupButtons();
        }

        protected override void SetupButtons()
        {
            base.SetupButtons();

            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(() =>
                {
                    SoundManager.Instance.SaveSettings();
                    Events.UIEvents.CloseSettingPanel();
                });
            }
        }
    }
}