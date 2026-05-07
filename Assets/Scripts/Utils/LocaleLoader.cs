using UnityEngine.Localization.Settings;

namespace Utils
{
    public static class LocaleLoader
    {
        readonly static string commonTextTable = "CommonTextTable";
        readonly static string popupMessageTable = "PopupMessageTable";

        public static string GetCommonText(string key)
        {
            return GetText(key, commonTextTable);
        }

        public static string GetPopupMessage(string key)
        {
            return GetText(key, popupMessageTable);
        }

        private static string GetText(string key, string table)
        {
            return LocalizationSettings.StringDatabase.GetLocalizedString(table, key);
        }
    }
}