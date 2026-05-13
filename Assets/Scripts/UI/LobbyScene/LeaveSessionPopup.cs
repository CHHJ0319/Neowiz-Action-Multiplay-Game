namespace UI.LobbyScene
{
    public class LeaveSessionPopup : UI.Common.CommonPopup
    {
        void Awake()
        {
            SetupButtons();
        }

        public override void Initialize()
        {
            string message = "";
            if(Services.NetworkService.IsHost())
            {
                message = Utils.LocaleLoader.GetConnectionMessage("CONFIRM_HOST_LEAVE");
            }
            else
            {
                message = Utils.LocaleLoader.GetConnectionMessage("CONFIRM_CLIENT_LEAVE");
            }
            SetMessage(message);
        }

        protected override void SetupButtons()
        {
            base.SetupButtons();

            if (confirmButton != null)
            {
                confirmButton.onClick.AddListener(() =>
                {
                    GameManager.Instance.Disconnect();
                });
            }
        }
    }
}