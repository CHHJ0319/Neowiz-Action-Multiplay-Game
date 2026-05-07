using UnityEngine.Localization.Settings;

namespace UI.TitleScene
{
    public class SaveSettingPopup : UI.LobbyScene.CommonPopup
    {
        void Awake()
        {
            string message = Utils.LocaleLoader.GetPopupMessage("Test");
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