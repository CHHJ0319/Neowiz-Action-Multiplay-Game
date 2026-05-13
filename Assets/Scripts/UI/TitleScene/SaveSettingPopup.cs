namespace UI.TitleScene
{
    public class SaveSettingPopup : UI.Common.CommonPopup
    {
        void Awake()
        {
            string message = Utils.LocaleLoader.GetPopupMessage("SaveSettingMessage");
            SetMessage(message);

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