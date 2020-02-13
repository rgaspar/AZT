using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public static class StringExtensions
    {
        public static readonly char[] DefaultCsvSeparatorChars = new char[] { ';' };

        public static string GetValueOrDefault(this string value, string defaultValue = null)
        {
            return String.IsNullOrWhiteSpace(value) ? defaultValue : value;
        }

        public static string EnsureSingleUrlSeparatorChars(this string value)
        {
            const string DuplicateUrlSeparatorChar = "//";
            const string SingleUrlSeparatorChar = "/";

            while (value.Contains(DuplicateUrlSeparatorChar))
            {
                value = value.Replace(DuplicateUrlSeparatorChar, SingleUrlSeparatorChar);
            }

            return value;
        }

        public static DateTime ParseDateTime(this string value, string format)
        {
            var result = DateTime.ParseExact(value, format, CultureInfo.InvariantCulture);

            return result;
        }

        public static string[] ConvertCsvToStringArray(this string value)
        {
            return ConvertCsvToStringArray(value, StringSplitOptions.RemoveEmptyEntries, DefaultCsvSeparatorChars);
        }

        public static string[] ConvertCsvToStringArray(this string value, StringSplitOptions options, params char[] separators)
        {
            if (separators.IsNullOrEmpty())
            {
                separators = DefaultCsvSeparatorChars;
            }

            return value.Split(separators, options);
        }

        public static string StripPrefix(this string value, string prefix)
        {
            return value.StartsWith(prefix) ? value.Substring(prefix.Length) : value;
        }
    }
}
