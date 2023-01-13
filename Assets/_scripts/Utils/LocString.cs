using System;
using UnityEngine.Localization;

namespace vgwb.utilities
{
    [Serializable]
    public struct LocString
    {
        public UnityEngine.Localization.LocalizedString Key;
        public string DefaultText;

        public LocString(string table, string locKey, string defaultText = "") : this()
        {
            Key = new LocalizedString(table, locKey);
            DefaultText = defaultText;
        }

        public string Text
        {
            get {
                if (Key != null && !Key.IsEmpty)
                    return Key.GetLocalizedString();
                return DefaultText;
            }
        }

        public static LocalizedString FromStr(string completeKey)
        {
            var splits = completeKey.Split("/");
            return new LocalizedString(splits[0], splits[1]);
        }
    }
}
