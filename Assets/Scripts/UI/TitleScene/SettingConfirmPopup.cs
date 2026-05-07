namespace UI.TitleScene
{
    public class SettingConfirmPopup: UI.Common.CommonPopup
    {
        void Awake()
        {
            string message = Utils.LocaleLoader.GetPopupMessage("SettingConfirmMessage"); 
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
                    SoundManager.Instance.LoadSettings();
                    Events.UIEvents.CloseSettingPanel();
                });
            }
        }
    }
}