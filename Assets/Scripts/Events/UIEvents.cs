using System;

namespace Events
{

    public static class UIEvents
    {
        public static event Action<string> OnJoinCodeGenerated;

        public static void Clear()
        {
            OnJoinCodeGenerated = null;
        }

        public static void SetJoinCode(string joinCode)
        {
            OnJoinCodeGenerated?.Invoke(joinCode);
        }
    }
}
