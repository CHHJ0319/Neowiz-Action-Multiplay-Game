using System;

namespace Events
{

    public static class UIEvents
    {
        public static event Action<string> OnJoinCodeGenerated;
        public static event Action OnSettingConfirmPanelClosed;

        public static void Clear()
        {
            OnJoinCodeGenerated = null;
            OnSettingConfirmPanelClosed = null;
        }

        public static void SetJoinCode(string joinCode)
        {
            OnJoinCodeGenerated?.Invoke(joinCode);
        }

        public static void CloseSettingPanel()
        {
            OnSettingConfirmPanelClosed?.Invoke();
        }

    }
}
