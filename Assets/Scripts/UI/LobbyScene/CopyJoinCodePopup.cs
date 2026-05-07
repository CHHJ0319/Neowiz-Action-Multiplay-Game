using UnityEngine;

namespace UI.LobbyScene
{
    public class CopyJoinCodePopup : UI.Common.CommonPopup
    {
        void Awake()
        {
            string message = Utils.LocaleLoader.GetPopupMessage("CopyJoinCodeMessage");
            SetPrompt(message);

            SetupButtons();
        }

        protected override void SetupButtons()
        {
            base.SetupButtons();
        }
    }
}
