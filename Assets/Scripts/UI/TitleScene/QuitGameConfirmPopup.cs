namespace UI.TitleScene
{
    public class QuitGameConfirmPopup : UI.Common.CommonPopup
    {
        void Awake()
        {
            string message = Utils.LocaleLoader.GetPopupMessage("QuitGameConfirmMessage"); 
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
                    Events.GameEvents.QuitGame();
                });
            }
        }
    }
}