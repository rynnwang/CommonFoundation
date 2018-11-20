using System;

namespace Beyova
{
    /// <summary>
    ///
    /// </summary>
    public static partial class CharactorStandardization
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="potentialDoubleByteSymbol">The potential double byte symbol.</param>
        /// <returns></returns>
        private delegate char ConvertDoubleByteToSingleByteChar(char potentialDoubleByteSymbol);

        /// <summary>
        /// To the single byte symbol. Specifically maps to single byte by meaningfully.
        /// </summary>
        /// <param name="potentialDoubleByteSymbol">The potential double byte symbol.</param>
        /// <returns></returns>
        public static char ToSingleByteSymbolMeaningfully(this char potentialDoubleByteSymbol)
        {
            Char result = potentialDoubleByteSymbol;

            // You can see detail via https://unicode-table.com/en/{Hex}/, like https://unicode-table.com/en/201F/
            switch (potentialDoubleByteSymbol)
            {
                case '\u3002':
                case '\uFF0E':
                    result = '.';
                    break;

                case '\uFF1F':
                    result = '?';
                    break;

                case '\uFF01':
                    result = '!';
                    break;

                case '\uFF0C':
                    result = ',';
                    break;

                case '\uFF1B':
                    result = ';';
                    break;

                case '\u3003':
                case '\uFF02':
                case '\u201C':
                case '\u201D':
                case '\u201E':
                case '\u201F':
                    result = '"';
                    break;

                case '\u3008':
                case '\uFF1C':
                    result = '<';
                    break;

                case '\u3009':
                case '\uFF1E':
                    result = '>';
                    break;

                case '\u3014':
                case '\uFF08':
                    result = '(';
                    break;

                case '\u3015':
                case '\uFF09':
                    result = ')';
                    break;

                case '\uFF1A':
                    result = ':';
                    break;

                case '\uFF1D':
                    result = '=';
                    break;

                case '\uFF20':
                    result = '@';
                    break;

                case '\uFF3B':
                    result = '[';
                    break;

                case '\uFF3D':
                    result = ']';
                    break;

                case '\uFF3C':
                    result = '\\';
                    break;

                case '\uFF0F':
                    result = '/';
                    break;

                case '\uFF3F':
                    result = '_';
                    break;

                case '\uFF5E':
                    result = '~';
                    break;

                case '\uFF5B':
                    result = '{';
                    break;

                case '\uFF5D':
                    result = '}';
                    break;

                case '\uFF04':
                    result = '$';
                    break;

                case '\uFF05':
                    result = '%';
                    break;

                case '\uFF06':
                    result = '&';
                    break;

                case '\uFF07':
                case '\u2018':
                case '\u2019':
                case '\u201A':
                case '\u201B':
                    result = '\'';
                    break;

                case '\uFF03':
                    result = '#';
                    break;

                case '\uFF0B':
                    result = '+';
                    break;

                case '\uFF0D':
                    result = '-';
                    break;

                case '\u3000':
                case '\u2002':
                case '\u2003':
                case '\u2007':
                case '\u2008':
                case '\u2009':
                case '\u200A':
                    result = ' ';
                    break;

                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// To the single byte symbol generally. Range map from Double-Byte to Single-Byte with offset 65248.
        /// </summary>
        /// <param name="potentialDoubleByteSymbol">The potential double byte symbolc.</param>
        /// <returns></returns>
        public static char ToSingleByteSymbolGenerally(this char potentialDoubleByteSymbol)
        {
            byte[] bytes = System.Text.Encoding.Unicode.GetBytes(potentialDoubleByteSymbol.ToString());

            int H = Convert.ToInt32(bytes[1]);
            int L = Convert.ToInt32(bytes[0]);

            int value = H * 256 + L;

            // Double byte
            if (value >= 65281 && value <= 65374)
            {
                int halfvalue = value - 65248;//65248 is the offset.
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else if (value == 12288)
            {
                int halfvalue = 32;
                byte halfL = Convert.ToByte(halfvalue);

                bytes[0] = halfL;
                bytes[1] = 0;
            }
            else
            {
                return potentialDoubleByteSymbol;
            }

            string ret = System.Text.Encoding.Unicode.GetString(bytes);
            return Convert.ToChar(ret);
        }

        /// <summary>
        /// To the single byte string meaningfully.
        /// </summary>
        /// <param name="potentialDoubleByteString">The potential double byte string.</param>
        /// <returns></returns>
        public static string ToSingleByteStringMeaningfully(this string potentialDoubleByteString)
        {
            return ToSingleByteString(potentialDoubleByteString, ToSingleByteSymbolMeaningfully);
        }

        /// <summary>
        /// To the single byte string generally.
        /// </summary>
        /// <param name="potentialDoubleByteString">The potential double byte string.</param>
        /// <returns></returns>
        public static string ToSingleByteStringGenerally(this string potentialDoubleByteString)
        {
            return ToSingleByteString(potentialDoubleByteString, ToSingleByteSymbolGenerally);
        }

        /// <summary>
        /// To the single byte string.
        /// </summary>
        /// <param name="potentialDoubleByteString">The potential double byte string.</param>
        /// <param name="converter">The converter.</param>
        /// <returns></returns>
        private static string ToSingleByteString(this string potentialDoubleByteString, ConvertDoubleByteToSingleByteChar converter)
        {
            if (string.IsNullOrWhiteSpace(potentialDoubleByteString))
            {
                return potentialDoubleByteString;
            }

            char[] result = new char[potentialDoubleByteString.Length];

            var i = 0;
            foreach (var one in potentialDoubleByteString)
            {
                result[i] = converter(one);
                i++;
            }

            return new string(result);
        }
    }
}