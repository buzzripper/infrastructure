using System;
using System.Text;

namespace ProData.Infrastructure.Common.Extensions
{
    public static class StringExtensions
    {
        public static byte[] ToBytes(this string str)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            return encoding.GetBytes(str);
        }

        public static string CamelCase(this string str)
        {
            if (string.IsNullOrWhiteSpace(str))
            {
                return str;
            }

            return $"{str.Substring(0, 1).ToLower()}{str.Substring(1)}";
        }

        public static bool EqualsIgnoreCase(this string str, string compareStr)
        {
            return string.Equals(str, compareStr, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this Guid guid, string compareStr)
        {
            return string.Equals(guid.ToString(), compareStr, StringComparison.OrdinalIgnoreCase);
        }

        public static bool EqualsIgnoreCase(this string str, Guid guid)
        {
            return string.Equals(guid.ToString(), str, StringComparison.OrdinalIgnoreCase);
        }

        public static bool ContainsIgnoreCase(this string text, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison) >= 0;
        }

        public static int IndexOfIgnoreCase(this string text, string value, StringComparison stringComparison = StringComparison.CurrentCultureIgnoreCase)
        {
            return text.IndexOf(value, stringComparison);
        }
    }
}