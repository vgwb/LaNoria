using System.Collections.Generic;
using System.Text;

namespace vgwb.framework
{
    public static class StringUtilities
    {
        public static bool IsNullOrEmpty(this string str)
        {
            return str is null or "" or "\u200B";
        }

        public static string ToJoinedString<T>(this IEnumerable<T> list)
        {
            var s = new StringBuilder();
            foreach (var v in list)
                s.AppendLine($" {v}");
            return s.ToString();
        }
    }
}
