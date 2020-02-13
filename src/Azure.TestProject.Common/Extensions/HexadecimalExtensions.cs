using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Azure.TestProject.Common
{
    public static class HexadecimalExtensions
    {
        private const int CharsPerByteInHexadecimalFormat = 2;

        private const int BitsPerHexadecimalDigit = 4;

        private const int LowestHexadecimalDigitMask = 0x0F;

        private const int LetterAOffset = 10;

        private const char DigitZero = '0';

        private const char DigitNine = '9';

        private const char UpperLetterA = 'A';

        private const char UpperLetterF = 'F';

        private const char LowerLetterA = 'a';

        private const char LowerLetterF = 'f';

        private const string RegexPattern = @"^[a-f\d]+$";

        private static readonly RegexOptions regexOptions = RegexOptions.IgnoreCase | RegexOptions.Singleline;

        private static readonly string[] lowerHexadecimalChars =
            Enumerable
                .Range(Byte.MinValue, Byte.MaxValue + 1)
                .Select(value => value.ToString(HexadecimalFormats.LowerCaseForByte))
                .ToArray();

        private static readonly string[] upperHexadecimalChars =
            Enumerable
                .Range(Byte.MinValue, Byte.MaxValue + 1)
                .Select(value => value.ToString(HexadecimalFormats.UpperCaseForByte))
                .ToArray();

        public static string ToHexadecimalString(this byte[] byteArray, bool useFasterMethod = true, bool upperCase = false)
        {
            if (byteArray.IsNullOrEmpty())
            {
                return String.Empty;
            }

            return
                useFasterMethod
                    ? FastConvertByteArrayToHexadecimalString(byteArray, upperCase)
                    : ConvertByteArrayToHexadecimalString(byteArray, upperCase);
        }

        private static string ConvertByteArrayToHexadecimalString(byte[] byteArray, bool upperCase)
        {
            var sb = new StringBuilder(byteArray.Length * CharsPerByteInHexadecimalFormat);

            foreach (byte @byte in byteArray)
            {
                sb.Append(upperCase ? upperHexadecimalChars[@byte] : lowerHexadecimalChars[@byte]);
            }

            return sb.ToString();
        }

        private static string FastConvertByteArrayToHexadecimalString(byte[] byteArray, bool upperCase)
        {
            char letterA = upperCase ? UpperLetterA : LowerLetterA;

            int byteArrayLength = byteArray.Length;

            char[] chars = new char[byteArrayLength * CharsPerByteInHexadecimalFormat];

            byte @byte;

            for (int byteIndex = 0, charIndex = 0; byteIndex < byteArrayLength; ++byteIndex, ++charIndex)
            {
                @byte = (byte)(byteArray[byteIndex] >> BitsPerHexadecimalDigit);
                chars[charIndex] = (char)(@byte > 9 ? @byte - 10 + letterA : @byte + DigitZero);

                @byte = (byte)(byteArray[byteIndex] & LowestHexadecimalDigitMask);
                chars[++charIndex] = (char)(@byte > 9 ? @byte - 10 + letterA : @byte + DigitZero);
            }

            return new string(chars);
        }
    }
}
